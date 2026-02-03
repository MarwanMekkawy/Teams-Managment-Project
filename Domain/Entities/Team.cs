

namespace Domain.Entities
{
    public class Team : BaseEntity<int>
    {
        public required string Name { get; set; }
        public int OrganizationId { get;  set; }

        // Navigation
        public required Organization Organization { get; set; }
        public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
