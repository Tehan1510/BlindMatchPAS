using BlindMatchPAS.Data;
using BlindMatchPAS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public string DetermineRole(string email)
        {
            email = email.ToLower().Trim();

            if (email.EndsWith("@student.nsbm.ac.lk"))
                return "Student";
            if (email.EndsWith("@supervisor.nsbm.ac.lk"))
                return "Supervisor";
            if (email.EndsWith("@admin.nsbm.ac.lk"))
                return "Admin";

            return string.Empty;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email.ToLower().Trim());
        }

        public async Task<bool> RegisterUserAsync(
            string fullName, string email, string password, string role)
        {
            if (await EmailExistsAsync(email))
                return false;

            var user = new User
            {
                FullName = fullName,
                Email = email.ToLower().Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (role == "Student")
            {
                _context.StudentProfiles.Add(
                    new StudentProfile { UserId = user.Id });
            }
            else if (role == "Supervisor")
            {
                _context.SupervisorProfiles.Add(
                    new SupervisorProfile
                    {
                        UserId = user.Id,
                        Department = "Faculty of Computing",
                        MaxQuota = 5,
                        PreferredAreas = string.Empty
                    });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            // Find user by email - case insensitive
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email.ToLower().Trim());

            if (user == null)
                return null;

            // Verify password against stored hash
            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            return passwordValid ? user : null;
        }
    }
}