using Domain.Enums;


namespace Shared.ProjectDTOs
{
    public class CreateProjectDto        
    {
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public string TeamName { get; set; }
    }
}
