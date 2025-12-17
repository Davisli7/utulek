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
        IList<AdoptionRequest> Select(string? searchEmail = null, AdoptionRequestStatus? statusFilter = null);
        IList<AdoptionRequest> SelectForUser(int userId);

        string? Create(int userId, int animalId);

        void UpdateStatus(int requestId, AdoptionRequestStatus newStatus);

        bool CancelRequest(int requestId, int userId);

        HashSet<int> GetActiveAnimalIdsForUser(int userId);
    }
}
