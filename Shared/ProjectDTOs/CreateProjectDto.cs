using Domain.Enums;


namespace Shared.ProjectDTOs
{
    public class CreateProjectDto        
    {
        public string Name { get; set; } 
        public ProjectStatus Status { get; set; }
        public int TeamId { get; set; }
    }
}
