
namespace Domain.Entities
{
    public class TeamMember
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }

        // Navigation
        public Team Team { get; set; }
        public User User { get; set; }
    }
}
