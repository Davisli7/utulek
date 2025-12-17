using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Enums;
using User = Utulek1.Domain.Entities.User;

namespace Utulek1.Domain.Entities
{
    public class AdoptionRequest
    {
        public int AdoptionRequestID { get; set; }

        public int AnimalID { get; set; }
        [ForeignKey(nameof(AnimalID))]
        public virtual Animal Animal { get; set; }

        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }

        public AdoptionRequestStatus Status { get; set; } = AdoptionRequestStatus.Pending; 

        public DateTime CreatedAt { get; set; } = DateTime.Now; 
    }

}
