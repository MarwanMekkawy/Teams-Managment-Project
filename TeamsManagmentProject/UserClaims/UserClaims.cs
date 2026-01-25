using Domain.Enums;
using System.Security.Claims;

namespace TeamsManagmentProject.API.UserClaims
{
    public record UserClaims(int UserId, int OrgId, UserRole Role)
    {
        public static UserClaims From(ClaimsPrincipal user)
        {
            return new UserClaims(
                int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                int.Parse(user.FindFirst("OrgId")!.Value),
                Enum.Parse<UserRole>(user.FindFirst(ClaimTypes.Role)!.Value)
            );
        }
    }
}
