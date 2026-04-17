using BlindMatchPAS.Data;
using BlindMatchPAS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Services
{
    public class MatchService : IMatchService
    {
        private readonly AppDbContext _context;

        public MatchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ConfirmMatchAsync(int projectId, int supervisorProfileId)
        {
            var project = await _context.Projects
                .Include(p => p.Match)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null || project.Status == "Matched" || project.Status == "Withdrawn")
                return false;

            // Check supervisor quota
            var supervisor = await _context.SupervisorProfiles
                .Include(s => s.Matches)
                .FirstOrDefaultAsync(s => s.Id == supervisorProfileId);

            if (supervisor == null) return false;

            int confirmedCount = supervisor.Matches
                .Count(m => m.Status == "Confirmed");

            if (confirmedCount >= supervisor.MaxQuota)
                return false;

            // Create the match and reveal identities
            var match = new Match
            {
                ProjectId = projectId,
                SupervisorProfileId = supervisorProfileId,
                Status = "Confirmed",
                IdentityRevealed = true,
                MatchedAt = DateTime.UtcNow
            };

            project.Status = "Matched";

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Match>> GetMatchesBySupervisorAsync(int supervisorProfileId)
        {
            return await _context.Matches
                .Include(m => m.Project)
                    .ThenInclude(p => p.StudentProfile)
                        .ThenInclude(s => s.User)
                .Where(m => m.SupervisorProfileId == supervisorProfileId)
                .ToListAsync();
        }

        public async Task<List<Match>> GetAllMatchesAsync()
        {
            return await _context.Matches
                .Include(m => m.Project)
                    .ThenInclude(p => p.StudentProfile)
                        .ThenInclude(s => s.User)
                .Include(m => m.SupervisorProfile)
                    .ThenInclude(s => s.User)
                .ToListAsync();
        }
    }
}