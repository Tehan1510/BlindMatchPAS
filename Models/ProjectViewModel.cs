namespace BlindMatchPAS.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public string ResearchArea { get; set; } = string.Empty;
        public string TechnicalStack { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }

        // Only populated AFTER identity is revealed
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
        public string? SupervisorName { get; set; }
        public string? SupervisorEmail { get; set; }
    }
}