namespace BlindMatchPAS.Models.Entities
{
    public class Match
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int SupervisorProfileId { get; set; }
        public SupervisorProfile SupervisorProfile { get; set; } = null!;

        // "Pending", "Confirmed"
        public string Status { get; set; } = "Pending";

        // False = identities hidden, True = identities revealed
        public bool IdentityRevealed { get; set; } = false;

        public DateTime MatchedAt { get; set; } = DateTime.UtcNow;
    }
}