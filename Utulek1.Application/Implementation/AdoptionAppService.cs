using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Domain.Enums;
using Utulek1.Infrastructure;

namespace Utulek1.Application.Implementation
{
    public class AdoptionAppService : IAdoptionAppService
    {
        private readonly UtulekDbContext _dbContext;

        public AdoptionAppService(UtulekDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<AdoptionRequest> Select()
        {
            // Načteme žádosti i s informacemi o Zvířeti a Uživateli
            return _dbContext.AdoptionRequests
                             .Include(ar => ar.Animal)
                             .Include(ar => ar.User)
                             .OrderByDescending(ar => ar.CreatedAt)
                             .ToList();
        }

        public IList<AdoptionRequest> SelectForUser(int userId)
        {
            return _dbContext.AdoptionRequests
                             .Include(ar => ar.Animal) // Načteme i info o zvířeti (kvůli jménu a fotce)
                             .Where(ar => ar.UserID == userId)
                             .OrderByDescending(ar => ar.CreatedAt)
                             .ToList();
        }

        public string? Create(int userId, int animalId)
        {
            // 1. KONTROLA: Má uživatel aktivní žádost? (Pending nebo Approved)
            bool hasActiveRequest = _dbContext.AdoptionRequests.Any(ar =>
                ar.UserID == userId &&
                (ar.Status == AdoptionRequestStatus.Pending || ar.Status == AdoptionRequestStatus.Approved));

            if (hasActiveRequest)
            {
                return "Můžete mít pouze jednu aktivní žádost o adopci.";
            }

            // 2. KONTROLA: Je zvíře stále k dispozici?
            var animal = _dbContext.Animals.Find(animalId);
            if (animal == null || animal.Status != "K adopci")
            {
                return "Toto zvíře již není k dispozici pro adopci.";
            }

            // 3. VYTVOŘENÍ ŽÁDOSTI
            var request = new AdoptionRequest
            {
                UserID = userId,
                AnimalID = animalId,
                Status = AdoptionRequestStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _dbContext.AdoptionRequests.Add(request);
            _dbContext.SaveChanges();

            return null; // Žádná chyba = úspěch
        }

        public void UpdateStatus(int requestId, AdoptionRequestStatus newStatus)
        {
            var request = _dbContext.AdoptionRequests
                                    .Include(ar => ar.Animal)
                                    .FirstOrDefault(ar => ar.AdoptionRequestID == requestId);

            if (request != null)
            {
                // Změna statusu samotné žádosti
                request.Status = newStatus;

                // --- SYNCHRONIZACE STATUSU ZVÍŘETE ---
                // Pokud byla žádost SCHVÁLENA -> Zvíře je REZERVOVÁNO
                if (newStatus == AdoptionRequestStatus.Approved)
                {
                    if (request.Animal != null) request.Animal.Status = "Zarezervováno";
                }
                // Pokud byla adopce DOKONČENA -> Zvíře je ADOPTOVÁNO
                else if (newStatus == AdoptionRequestStatus.Completed)
                {
                    if (request.Animal != null) request.Animal.Status = "Adoptováno";
                }
                // Pokud byla ZRUŠENA nebo ZAMÍTNUTA a zvíře bylo rezervované touto žádostí -> Zpět k ADOPCI
                else if ((newStatus == AdoptionRequestStatus.Rejected || newStatus == AdoptionRequestStatus.Cancelled)
                         && request.Animal.Status == "Zarezervováno")
                {
                    // Tady by šlo přidat logiku, že to vrátíme jen pokud to nezarezervoval mezitím někdo jiný (což by neměl)
                    request.Animal.Status = "K adopci";
                }

                _dbContext.SaveChanges();
            }
        }

        public bool CancelRequest(int requestId, int userId)
        {
            // Najdeme žádost i se zvířetem
            var request = _dbContext.AdoptionRequests
                                  .Include(ar => ar.Animal)
                                  .FirstOrDefault(ar => ar.AdoptionRequestID == requestId);

            // Kontrola: Existuje? A patří opravdu tomu přihlášenému uživateli?
            if (request == null || request.UserID != userId)
            {
                return false; // Neoprávněný přístup nebo neexistuje
            }

            // Můžeme zrušit jen aktivní žádosti (ne ty, co už jsou dokončené nebo zamítnuté)
            if (request.Status == AdoptionRequestStatus.Completed || request.Status == AdoptionRequestStatus.Rejected)
            {
                return false;
            }

            // Změna statusu žádosti
            request.Status = AdoptionRequestStatus.Cancelled;

            // SYNCHRONIZACE: Pokud bylo zvíře "Zarezervováno" touto žádostí, musíme ho uvolnit
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