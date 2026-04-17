using BlindMatchPAS.Data;
using BlindMatchPAS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAvailableProjectsAsync(string filter = "All")
        {
            var query = _context.Projects
                .Where(p => p.Status == "Pending" || p.Status == "UnderReview");

            if (filter != "All")
                query = query.Where(p => p.ResearchArea == filter);

            return await query.ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.StudentProfile)
                    .ThenInclude(s => s.User)
                .Include(p => p.Match)
                    .ThenInclude(m => m!.SupervisorProfile)
                        .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Project>> GetProjectsByStudentAsync(int studentProfileId)
        {
            return await _context.Projects
                .Include(p => p.Match)
                    .ThenInclude(m => m!.SupervisorProfile)
                        .ThenInclude(s => s.User)
                .Where(p => p.StudentProfileId == studentProfileId)
                .ToListAsync();
        }

        public async Task<bool> SubmitProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            var existing = await _context.Projects.FindAsync(project.Id);
            if (existing == null || existing.Status == "Matched")
                return false;

            existing.Title = project.Title;
            existing.Abstract = project.Abstract;
            existing.TechnicalStack = project.TechnicalStack;
            existing.ResearchArea = project.ResearchArea;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WithdrawProjectAsync(int projectId, int studentProfileId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId
                    && p.StudentProfileId == studentProfileId);

            if (project == null || project.Status == "Matched")
                return false;

            project.Status = "Withdrawn";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}