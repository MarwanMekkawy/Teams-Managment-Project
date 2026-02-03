using Domain.Enums;

namespace Shared.UserDTOs
{
    public class CreateUserDto          
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public UserRole Role { get; set; } = UserRole.Member;
        public int OrganizationId { get; set; }
        public required string Password { get; set; }
    }
}
