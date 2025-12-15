using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utulek1.Application.Abstraction;
using Utulek1.Application.ViewModels;
using Utulek1.Domain.Entities;

namespace Utulek1.Controllers
{
    public class AdoptionController : Controller
    {
        private readonly IAnimalAppService _animalAppService;
        private readonly IAdoptionAppService _adoptionAppService; // Nová služba
        private readonly UserManager<User> _userManager;          // Pro získání ID uživatele

        public AdoptionController(IAnimalAppService animalAppService,
                                  IAdoptionAppService adoptionAppService,
                                  UserManager<User> userManager)
        {
            _animalAppService = animalAppService;
            _adoptionAppService = adoptionAppService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Načteme zvířata
            IList<Animal> animals = _animalAppService.Select();

            // 2. Připravíme ViewModel
            var viewModel = new AdoptionIndexViewModel
            {
                Animals = animals
            };

            // 3. Pokud je uživatel přihlášený, zjistíme jeho aktivní žádosti
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    viewModel.UserRequestedAnimalIds = _adoptionAppService.GetActiveAnimalIdsForUser(user.Id);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            // 1. Načteme detail zvířete (včetně fotek)
            var animal = _animalAppService.Select(id);

            if (animal == null)
            {
                return NotFound();
            }

            // 2. Připravíme ViewModel
            var viewModel = new AdoptionDetailViewModel
            {
                Animal = animal
            };

            // 3. Zjistíme, jestli má uživatel o toto konkrétní zvíře zájem
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Využijeme tvoji existující metodu pro získání IDček
                    var activeRequests = _adoptionAppService.GetActiveAnimalIdsForUser(user.Id);

                    // Pokud je ID tohoto zvířete v seznamu, nastavíme true
                    if (activeRequests.Contains(animal.AnimalID))
                    {
                        viewModel.HasActiveRequest = true;
                    }
                }
            }

            return View(viewModel);
        }

        // --- AKCE PRO VYTVOŘENÍ ŽÁDOSTI ---
        [HttpPost]
        [Authorize] // Jen pro přihlášené
        public async Task<IActionResult> CreateRequest(int animalId)
        {
            // Získáme aktuálního uživatele
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            // Pokusíme se vytvořit žádost
            string? error = _adoptionAppService.Create(user.Id, animalId);

            if (error != null)
            {
                // Pokud nastala chyba (např. už má aktivní žádost), zobrazíme ji
                TempData["ErrorMessage"] = error;
            }
            else
            {
                // Úspěch
                TempData["SuccessMessage"] = "Vaše žádost o adopci byla úspěšně vytvořena.";
                return RedirectToAction(nameof(MyAdoptions));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cancel(int requestId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            bool success = _adoptionAppService.CancelRequest(requestId, user.Id);

            if (success)
            {
                TempData["SuccessMessage"] = "Žádost byla úspěšně zrušena.";
            }
            else
            {
                TempData["ErrorMessage"] = "Žádost se nepodařilo zrušit.";
            }

            return RedirectToAction(nameof(MyAdoptions));
        }

        // --- MOJE ADOPCE ---
        [Authorize]
        public async Task<IActionResult> MyAdoptions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var requests = _adoptionAppService.SelectForUser(user.Id);
            return View(requests);
        }
    }
}
