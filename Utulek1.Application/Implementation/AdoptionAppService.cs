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
using Utulek1.Infrastructure.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Utulek1.Application.Implementation
{
    public class AdoptionAppService : IAdoptionAppService
    {
        private readonly IAdoptionRepository _adoptionRepository;
        private readonly IAnimalRepository _animalRepository;

        public AdoptionAppService(IAdoptionRepository adoptionRepository, IAnimalRepository animalRepository)
        {
            _adoptionRepository = adoptionRepository;
            _animalRepository = animalRepository;
        }

        public IList<AdoptionRequest> Select(string? searchEmail = null, AdoptionRequestStatus? statusFilter = null)
        {
            return _adoptionRepository.Select(searchEmail, statusFilter);
        }

        public IList<AdoptionRequest> SelectForUser(int userId)
        {
            return _adoptionRepository.SelectForUser(userId);
        }

        public string? Create(int userId, int animalId)
        {
            if (_adoptionRepository.HasActiveRequest(userId))
            {
                return "Můžete mít pouze jednu aktivní žádost o adopci.";
            }

            var animal = _animalRepository.Select(animalId);
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

            _adoptionRepository.Create(request);
            return null;
        }

        public void UpdateStatus(int requestId, AdoptionRequestStatus newStatus)
        {
            var request = _adoptionRepository.GetById(requestId);
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
                         && request.Animal != null && request.Animal.Status == "Zarezervováno")
                {
                    request.Animal.Status = "K adopci";
                }

                _adoptionRepository.Update(request);
            }
        }

        public bool CancelRequest(int requestId, int userId)
        {
            var request = _adoptionRepository.GetById(requestId);

            if (request == null || request.UserID != userId) return false;
            if (request.Status == AdoptionRequestStatus.Completed || request.Status == AdoptionRequestStatus.Rejected) return false;

            request.Status = AdoptionRequestStatus.Cancelled;

            if (request.Animal != null && request.Animal.Status == "Zarezervováno")
            {
                request.Animal.Status = "K adopci";
            }

            _adoptionRepository.Update(request);
            return true;
        }

        public HashSet<int> GetActiveAnimalIdsForUser(int userId)
        {
            return _adoptionRepository.GetActiveAnimalIdsForUser(userId);
        }
    }
}