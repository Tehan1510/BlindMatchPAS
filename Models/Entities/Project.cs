using System.ComponentModel.DataAnnotations;

namespace BlindMatchPAS.Models.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Abstract { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TechnicalStack { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ResearchArea { get; set; } = string.Empty;

        // "Pending", "UnderReview", "Matched", "Withdrawn"
        public string Status { get; set; } = "Pending";

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; } = null!;

        // Navigation
        public Match? Match { get; set; }
    }
}