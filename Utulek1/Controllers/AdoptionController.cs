using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utulek1.Application.Abstraction;
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

        public IActionResult Index()
        {
            IList<Animal> animals = _animalAppService.Select();
            return View(animals);
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
