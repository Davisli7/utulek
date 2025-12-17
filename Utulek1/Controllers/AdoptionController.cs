using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private readonly ILogger<AdoptionController> _logger;

        public AdoptionController(IAnimalAppService animalAppService,
                                  IAdoptionAppService adoptionAppService,
                                  UserManager<User> userManager,
                                  ILogger<AdoptionController> logger)
        {
            _animalAppService = animalAppService;
            _adoptionAppService = adoptionAppService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchName, int? speciesId, string? status)
        {
            // 1. Získání seznamu druhů pro Dropdown
            ViewBag.SpeciesList = new SelectList(_animalAppService.SelectSpecies(), "SpeciesID", "Name", speciesId);

            // 2. Uložení filtrů do ViewBag (aby zůstaly vyplněné ve formuláři)
            ViewBag.CurrentSearchName = searchName;
            ViewBag.CurrentSpeciesId = speciesId;
            ViewBag.CurrentStatus = status;

            // 3. Načtení zvířat s použitím filtrů (metodu Select s parametry už v AnimalAppService máte)
            IList<Animal> animals = _animalAppService.Select(searchName, speciesId, status);

            // 4. Příprava ViewModelu
            var viewModel = new AdoptionIndexViewModel
            {
                Animals = animals
            };

            // 5. Zjištění aktivních žádostí přihlášeného uživatele
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
            try
            {
                // 1. Získáme aktuálního uživatele
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Challenge(); // Pokud není přihlášen, pošleme ho na login

                // 2. Pokusíme se vytvořit žádost přes službu
                // Poznámka: Předpokládám, že vaše metoda Create vrací string (chybovou hlášku) nebo null (úspěch)
                string? error = _adoptionAppService.Create(user.Id, animalId);

                if (error != null)
                {
                    // LOGOVÁNÍ VAROVÁNÍ (Business Logic Fail)
                    _logger.LogWarning("Uživatel '{UserName}' (ID: {UserId}) se pokusil o adopci zvířete ID {AnimalId}, ale neprošlo to. Důvod: {Reason}",
                                       user.UserName, user.Id, animalId, error);

                    TempData["ErrorMessage"] = error;
                }
                else
                {
                    // LOGOVÁNÍ ÚSPĚCHU (Info)
                    _logger.LogInformation("Uživatel '{UserName}' (ID: {UserId}) ÚSPĚŠNĚ vytvořil žádost o adopci zvířete ID {AnimalId}.",
                                           user.UserName, user.Id, animalId);

                    TempData["SuccessMessage"] = "Vaše žádost o adopci byla úspěšně vytvořena.";
                    return RedirectToAction(nameof(MyAdoptions));
                }
            }
            catch (Exception ex)
            {
                // LOGOVÁNÍ KRITICKÉ CHYBY (Error)
                _logger.LogError(ex, "Neočekávaná chyba při vytváření žádosti o adopci (AnimalID: {AnimalId}) uživatelem.", animalId);

                TempData["ErrorMessage"] = "Omlouváme se, došlo k neočekávané chybě. Zkuste to prosím později.";
            }

            // Pokud došlo k chybě, vrátíme uživatele zpět na nabídku
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
