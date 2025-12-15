using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Application.ViewModels
{
    public class AdoptionDetailViewModel
    {
        public Animal Animal { get; set; }
        public bool HasActiveRequest { get; set; } = false; // Má uživatel aktivní žádost o TOTO zvíře?
    }
}
