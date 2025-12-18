using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Domain.Enums;
using Utulek1.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Utulek1.Application.Implementation
{
    public class AdoptionAppService : IAdoptionAppService
    {
        private readonly UtulekDbContext _dbContext;

        public AdoptionAppService(UtulekDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<AdoptionRequest> Select(string? searchEmail = null, AdoptionRequestStatus? statusFilter = null)
        {
            IQueryable<AdoptionRequest> query = _dbContext.AdoptionRequests
                                                .Include(ar => ar.Animal)
                                                .ThenInclude(a => a.Photos)
                                                .Include(ar => ar.User); 

            if (!string.IsNullOrEmpty(searchEmail))
            {
                query = query.Where(ar => ar.User.Email.Contains(searchEmail));
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(ar => ar.Status == statusFilter.Value);
            }

            return query.OrderByDescending(ar => ar.CreatedAt).ToList();
        }

        public IList<AdoptionRequest> SelectForUser(int userId)
        {
            return _dbContext.AdoptionRequests
                             .Include(ar => ar.Animal)
                             .ThenInclude(a => a.Photos)
                             .Where(ar => ar.UserID == userId)
                             .OrderByDescending(ar => ar.CreatedAt)
                             .ToList();
        }

        public string? Create(int userId, int animalId)
        {
            bool hasActiveRequest = _dbContext.AdoptionRequests.Any(ar =>
                ar.UserID == userId &&
                (ar.Status == AdoptionRequestStatus.Pending || ar.Status == AdoptionRequestStatus.Approved));

            if (hasActiveRequest)
            {
                return "Můžete mít pouze jednu aktivní žádost o adopci.";
            }

            var animal = _dbContext.Animals.Find(animalId);
            if (animal == null || animal.Status != "K adopci")
            {
                return "Toto zvíře již není k dispozici pro adopci.";
            }

            var request = new AdoptionRequest
            {
                UserID = userId,
                AnimalID = animalId,
                Status = AdoptionRequestStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _dbContext.AdoptionRequests.Add(request);
            _dbContext.SaveChanges();

            return null; 
        }

        public void UpdateStatus(int requestId, AdoptionRequestStatus newStatus)
        {
            var request = _dbContext.AdoptionRequests
                                    .Include(ar => ar.Animal)
                                    .FirstOrDefault(ar => ar.AdoptionRequestID == requestId);

            if (request != null)
            {
                request.Status = newStatus;


                if (newStatus == AdoptionRequestStatus.Approved)
                {
                    if (request.Animal != null) request.Animal.Status = "Zarezervováno";
                }
                else if (newStatus == AdoptionRequestStatus.Completed)
                {
                    if (request.Animal != null) request.Animal.Status = "Adoptováno";
                }
                else if ((newStatus == AdoptionRequestStatus.Rejected || newStatus == AdoptionRequestStatus.Cancelled)
                         && request.Animal.Status == "Zarezervováno")
                {
                    request.Animal.Status = "K adopci";
                }

                _dbContext.SaveChanges();
            }
        }

        public bool CancelRequest(int requestId, int userId)
        {
            var request = _dbContext.AdoptionRequests
                                  .Include(ar => ar.Animal)
                                  .FirstOrDefault(ar => ar.AdoptionRequestID == requestId);

            if (request == null || request.UserID != userId)
            {
                return false; 
            }

            if (request.Status == AdoptionRequestStatus.Completed || request.Status == AdoptionRequestStatus.Rejected)
            {
                return false;
            }

            request.Status = AdoptionRequestStatus.Cancelled;

            if (request.Animal != null && request.Animal.Status == "Zarezervováno")
            {
                request.Animal.Status = "K adopci";
            }

            _dbContext.SaveChanges();
            return true;
        }

        public HashSet<int> GetActiveAnimalIdsForUser(int userId)
        {
            return _dbContext.AdoptionRequests
                             .Where(ar => ar.UserID == userId &&
                                         (ar.Status == AdoptionRequestStatus.Pending ||
                                          ar.Status == AdoptionRequestStatus.Approved))
                             .Select(ar => ar.AnimalID)
                             .ToHashSet();
        }

    }
}