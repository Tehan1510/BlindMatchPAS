namespace BlindMatchPAS.Models.Entities
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}