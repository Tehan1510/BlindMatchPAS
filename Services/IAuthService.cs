using BlindMatchPAS.Models.Entities;

namespace BlindMatchPAS.Services
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string email, string password);
        Task<bool> RegisterUserAsync(string fullName, string email, string password, string role);
        Task<bool> EmailExistsAsync(string email);
        string DetermineRole(string email);
    }
}