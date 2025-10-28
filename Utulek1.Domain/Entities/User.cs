using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } // User, Admin
        public DateTime CreatedAt { get; set; }
        public ICollection<AdoptionRequest> AdoptionRequests { get; set; }
    }
}
