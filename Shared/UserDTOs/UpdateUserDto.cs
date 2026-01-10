using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.UserDTOs
{
    public class UpdateUserDto          
    {
        public string Name { get; set; }
        public UserRole Role { get; set; }
    }
}
