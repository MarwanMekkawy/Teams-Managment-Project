
namespace Domain.Entities
{
    public class TeamMember
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }

        // Navigation
        public required Team Team { get; set; }
        public required User User { get; set; }
    }
}
