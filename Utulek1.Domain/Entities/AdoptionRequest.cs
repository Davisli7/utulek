using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Enums;

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

        // Nové vlastnosti
        public AdoptionRequestStatus Status { get; set; } = AdoptionRequestStatus.Pending; // Výchozí stav je "Čeká"

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Automaticky nastavíme čas vytvoření
    }

}
