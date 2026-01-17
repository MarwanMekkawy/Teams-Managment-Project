using Microsoft.AspNetCore.Mvc;

namespace TeamsManagmentProject.API.Controllers
{
    [ApiController]
    [Route("api/v1/teams/{teamId}/members")]
    [Route("api/v1/users/{userId}/teams", Name = "UserTeams")]
    public class TeamMembersController : ControllerBase
    {
    }
}
