namespace BlindMatchPAS.Models
{
    public class AdminMatchViewModel
    {
        public string StudentName { get; set; } = string.Empty;
        public string ProjectTitle { get; set; } = string.Empty;
        public string SupervisorName { get; set; } = string.Empty;
        public string ResearchArea { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime MatchedAt { get; set; }
    }

    public class UserManagementViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}