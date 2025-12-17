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
        private readonly IAdoptionAppService _adoptionAppService; 
        private readonly UserManager<User> _userManager;          

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
            ViewBag.SpeciesList = new SelectList(_animalAppService.SelectSpecies(), "SpeciesID", "Name", speciesId);

            ViewBag.CurrentSearchName = searchName;
            ViewBag.CurrentSpeciesId = speciesId;
            ViewBag.CurrentStatus = status;

            IList<Animal> animals = _animalAppService.Select(searchName, speciesId, status);

            var viewModel = new AdoptionIndexViewModel
            {
                Animals = animals
            };

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
            var animal = _animalAppService.Select(id);

            if (animal == null)
            {
                return NotFound();
            }

            var viewModel = new AdoptionDetailViewModel
            {
                Animal = animal
            };

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var activeRequests = _adoptionAppService.GetActiveAnimalIdsForUser(user.Id);

                    if (activeRequests.Contains(animal.AnimalID))
                    {
                        viewModel.HasActiveRequest = true;
                    }
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> CreateRequest(int animalId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Challenge(); 


                string? error = _adoptionAppService.Create(user.Id, animalId);

                if (error != null)
                {
                    _logger.LogWarning("Uživatel '{UserName}' (ID: {UserId}) se pokusil o adopci zvířete ID {AnimalId}, ale neprošlo to. Důvod: {Reason}",
                                       user.UserName, user.Id, animalId, error);

                    TempData["ErrorMessage"] = error;
                }
                else
                {
                    _logger.LogInformation("Uživatel '{UserName}' (ID: {UserId}) ÚSPĚŠNĚ vytvořil žádost o adopci zvířete ID {AnimalId}.",
                                           user.UserName, user.Id, animalId);

                    TempData["SuccessMessage"] = "Vaše žádost o adopci byla úspěšně vytvořena.";
                    return RedirectToAction(nameof(MyAdoptions));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Neočekávaná chyba při vytváření žádosti o adopci (AnimalID: {AnimalId}) uživatelem.", animalId);

                TempData["ErrorMessage"] = "Omlouváme se, došlo k neočekávané chybě. Zkuste to prosím později.";
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
