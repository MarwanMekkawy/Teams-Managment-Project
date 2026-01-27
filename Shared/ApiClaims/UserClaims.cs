using Domain.Enums;

namespace Shared.Claims
{
    public record UserClaims(int UserId, int OrgId, UserRole Role);   
}
