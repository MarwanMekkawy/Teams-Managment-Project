using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User: BaseEntity<int>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; } = UserRole.Member;

        public int? OrganizationId { get; set; }

        public string PasswordHash { get; set; }

        // Navigation
        public Organization? Organization { get; set; }
        public ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
        public ICollection<TaskEntity> AssignedTasks { get; set; } = new List<TaskEntity>();
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
    }
}
