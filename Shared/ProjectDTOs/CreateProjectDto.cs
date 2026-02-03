using Domain.Enums;

namespace Shared.ProjectDTOs
{
    public class CreateProjectDto        
    {
        public required string Name { get; set; } 
        public ProjectStatus Status { get; set; }
        public int TeamId { get; set; }
    }
}
