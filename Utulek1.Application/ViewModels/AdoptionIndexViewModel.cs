using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Application.ViewModels
{
    public class AdoptionIndexViewModel
    {
        public IList<Animal> Animals { get; set; } = new List<Animal>();

        // Seznam ID zvířat, o která uživatel už požádal.
        // Používáme HashSet pro super-rychlé vyhledávání.
        public HashSet<int> UserRequestedAnimalIds { get; set; } = new HashSet<int>();
    }
}
