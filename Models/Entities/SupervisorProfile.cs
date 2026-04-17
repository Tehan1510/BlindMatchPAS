using System.ComponentModel.DataAnnotations;

namespace BlindMatchPAS.Models.Entities
{
    public class SupervisorProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [StringLength(100)]
        public string Department { get; set; } = "Faculty of Computing";

        public int MaxQuota { get; set; } = 5;

        // Comma-separated preferred areas e.g. "AI,Software"
        public string PreferredAreas { get; set; } = string.Empty;

        // Navigation
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}