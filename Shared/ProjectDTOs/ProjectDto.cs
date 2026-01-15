using Domain.Enums;


namespace Shared.ProjectDTOs
{
    public class ProjectDto              
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }
        public int TeamId { get; set; }
    }
}
