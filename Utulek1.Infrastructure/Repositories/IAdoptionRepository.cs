using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;
using Utulek1.Domain.Enums;

namespace Utulek1.Infrastructure.Repositories
{
    public interface IAdoptionRepository
    {
        IList<AdoptionRequest> Select(string? searchEmail = null, AdoptionRequestStatus? statusFilter = null);
        IList<AdoptionRequest> SelectForUser(int userId);
        AdoptionRequest? GetById(int id);
        bool HasActiveRequest(int userId);
        void Create(AdoptionRequest request);
        void Update(AdoptionRequest request);
        HashSet<int> GetActiveAnimalIdsForUser(int userId);
    }
}
