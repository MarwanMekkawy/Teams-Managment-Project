using Domain.Enums;

namespace Shared.UserDTOs
{
    public class CreateUserDto          
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string OrganizationName { get; set; }
    }
}
