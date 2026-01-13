using Domain.Enums;


namespace Shared.ProjectDTOs
{
    public class ProjectDto              // GET /projects
    {
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public string TeamName { get; set; }
        public List<string>? TaskTitles { get; set; }
    }
}
