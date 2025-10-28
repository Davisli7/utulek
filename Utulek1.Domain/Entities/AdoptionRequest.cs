using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class AdoptionRequest
    {
        [Key]
        public int RequestID { get; set; } 
        public int AnimalID { get; set; }
        public Animal Animal { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } 
        public string Message { get; set; }
    }

}
