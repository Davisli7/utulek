using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;
using Utulek1.Domain.Enums;

namespace Utulek1.Infrastructure.Repositories
{
    public class AdoptionRepository : IAdoptionRepository
    {
        private readonly UtulekDbContext _dbContext;

        public AdoptionRepository(UtulekDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<AdoptionRequest> Select(string? searchEmail = null, AdoptionRequestStatus? statusFilter = null)
        {
            IQueryable<AdoptionRequest> query = _dbContext.AdoptionRequests
                .Include(ar => ar.Animal).ThenInclude(a => a.Photos)
                .Include(ar => ar.User);

            if (!string.IsNullOrEmpty(searchEmail))
                query = query.Where(ar => ar.User.Email.Contains(searchEmail));

            if (statusFilter.HasValue)
                query = query.Where(ar => ar.Status == statusFilter.Value);

            return query.OrderByDescending(ar => ar.CreatedAt).ToList();
        }

        public IList<AdoptionRequest> SelectForUser(int userId)
        {
            return _dbContext.AdoptionRequests
                .Include(ar => ar.Animal).ThenInclude(a => a.Photos)
                .Where(ar => ar.UserID == userId)
                .OrderByDescending(ar => ar.CreatedAt)
                .ToList();
        }

        public AdoptionRequest? GetById(int id)
        {
            return _dbContext.AdoptionRequests
                .Include(ar => ar.Animal)
                .FirstOrDefault(ar => ar.AdoptionRequestID == id);
        }

        public bool HasActiveRequest(int userId)
        {
            return _dbContext.AdoptionRequests.Any(ar =>
                ar.UserID == userId &&
                (ar.Status == AdoptionRequestStatus.Pending || ar.Status == AdoptionRequestStatus.Approved));
        }

        public void Create(AdoptionRequest request)
        {
            _dbContext.AdoptionRequests.Add(request);
            _dbContext.SaveChanges();
        }

        public void Update(AdoptionRequest request)
        {
            _dbContext.AdoptionRequests.Update(request);
            _dbContext.SaveChanges();
        }

        public HashSet<int> GetActiveAnimalIdsForUser(int userId)
        {
            return _dbContext.AdoptionRequests
                .Where(ar => ar.UserID == userId &&
                       (ar.Status == AdoptionRequestStatus.Pending || ar.Status == AdoptionRequestStatus.Approved))
                .Select(ar => ar.AnimalID)
                .ToHashSet();
        }
    }
}
