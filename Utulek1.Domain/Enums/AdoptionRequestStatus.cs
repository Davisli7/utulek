using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Enums
{
    public enum AdoptionRequestStatus
    {
        Pending,    // Čeká na schválení
        Approved,   // Schváleno (Zvíře je rezervováno)
        Rejected,   // Zamítnuto
        Completed,  // Dokončeno (Zvíře si odvezli)
        Cancelled   // Zrušeno uživatelem
    }
}
