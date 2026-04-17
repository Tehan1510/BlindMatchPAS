using System.ComponentModel.DataAnnotations;

namespace BlindMatchPAS.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
        // "Student", "Supervisor", "Admin"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public StudentProfile? StudentProfile { get; set; }
        public SupervisorProfile? SupervisorProfile { get; set; }
    }
}