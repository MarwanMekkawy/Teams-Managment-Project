using Domain.Enums;
using Shared.Claims;
using System.Security.Claims;

namespace TeamsManagmentProject.API.ApiClaimsFactory
{
    public static class UserClaimsFactory
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

