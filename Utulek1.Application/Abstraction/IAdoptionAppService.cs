using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;
using Utulek1.Domain.Enums;

namespace Utulek1.Application.Abstraction
{
    public interface IAdoptionAppService
    {
        // Vrátí všechny žádosti (pro admina)
        IList<AdoptionRequest> Select();

        // Vrátí žádosti konkrétního uživatele
        IList<AdoptionRequest> SelectForUser(int userId);

        // Vytvoří novou žádost. Vrací string s chybou (pokud nastane), nebo null (pokud je vše OK).
        string? Create(int userId, int animalId);

        // Změní status žádosti (schválit/zamítnout)
        void UpdateStatus(int requestId, AdoptionRequestStatus newStatus);

        // Zruší žádost (vrátí true, pokud se povedlo, false pokud ne - např. cizí žádost)
        bool CancelRequest(int requestId, int userId);

        // Vrátí množinu ID zvířat, u kterých má uživatel aktivní žádost
        HashSet<int> GetActiveAnimalIdsForUser(int userId);
    }
}
