using BlindMatchPAS.Models.Entities;

namespace BlindMatchPAS.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAvailableProjectsAsync(string filter = "All");
        Task<Project?> GetProjectByIdAsync(int id);
        Task<List<Project>> GetProjectsByStudentAsync(int studentProfileId);
        Task<bool> SubmitProjectAsync(Project project);
        Task<bool> UpdateProjectAsync(Project project);
        Task<bool> WithdrawProjectAsync(int projectId, int studentProfileId);
    }
}