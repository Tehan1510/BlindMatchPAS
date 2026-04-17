using BlindMatchPAS.Models.Entities;

namespace BlindMatchPAS.Services
{
    public interface IMatchService
    {
        Task<bool> ConfirmMatchAsync(int projectId, int supervisorProfileId);
        Task<List<Match>> GetMatchesBySupervisorAsync(int supervisorProfileId);
        Task<List<Match>> GetAllMatchesAsync();
    }
}