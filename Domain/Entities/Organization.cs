
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Organization:BaseEntity<int>
    {
        public string Name { get; set; }

        // Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
