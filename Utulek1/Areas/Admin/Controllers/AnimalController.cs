using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Nutné pro SelectList
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure; // Nutné pro DbContext

namespace Utulek1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Manager")]
    public class AnimalController : Controller
    {
        private readonly IAnimalAppService _animalAppService;
        private readonly UtulekDbContext _dbContext; // Přidán kontext pro načtení číselníků

        public AnimalController(IAnimalAppService animalAppService, UtulekDbContext dbContext)
        {
            _animalAppService = animalAppService;
            _dbContext = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Select()
        {

            IList<Animal> animals = _animalAppService.Select();
            return View(animals);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SpeciesList = new SelectList(_dbContext.Species, "SpeciesID", "Name");
            ViewBag.BreedList = new SelectList(_dbContext.Breeds, "BreedID", "Name");

            return View();
        }


        [HttpPost]
        public IActionResult Create(Animal animal, IEnumerable<IFormFile> uploadedFiles)
        {
            ModelState.Remove(nameof(Animal.Breed));
            ModelState.Remove(nameof(Animal.Species));
            ModelState.Remove(nameof(Animal.Photos));
            ModelState.Remove(nameof(Animal.AdoptionRequests));

            if (ModelState.IsValid)
            {
                _animalAppService.Create(animal, uploadedFiles);

                return RedirectToAction(nameof(AnimalController.Select));
            }

            ViewBag.SpeciesList = new SelectList(_dbContext.Species, "SpeciesID", "Name");
            ViewBag.BreedList = new SelectList(_dbContext.Breeds, "BreedID", "Name");

            return View(animal);
        }

        // --- EDITACE (GET) ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // 1. Najdeme zvíře podle ID
            Animal? animal = _animalAppService.Select(id);

            if (animal == null)
            {
                return NotFound();
            }

            // 2. Naplníme dropdowny (stejně jako u Create)
            ViewBag.SpeciesList = new SelectList(_dbContext.Species, "SpeciesID", "Name", animal.SpeciesID);
            ViewBag.BreedList = new SelectList(_dbContext.Breeds, "BreedID", "Name", animal.BreedID);

            // 3. Pošleme zvíře do View
            return View(animal);
        }

        // --- EDITACE (POST) ---
        [HttpPost]
        public IActionResult Edit(Animal animal, IEnumerable<IFormFile> uploadedFiles)
        {
            // Validace - odstraníme navigační vlastnosti
            ModelState.Remove(nameof(Animal.Breed));
            ModelState.Remove(nameof(Animal.Species));
            ModelState.Remove(nameof(Animal.Photos));
            ModelState.Remove(nameof(Animal.AdoptionRequests));

            if (ModelState.IsValid)
            {
                // Voláme Update metodu
                _animalAppService.Update(animal, uploadedFiles);
                return RedirectToAction(nameof(Select));
            }

            // Pokud je chyba, musíme znovu načíst dropdowny
            ViewBag.SpeciesList = new SelectList(_dbContext.Species, "SpeciesID", "Name", animal.SpeciesID);
            ViewBag.BreedList = new SelectList(_dbContext.Breeds, "BreedID", "Name", animal.BreedID);

            return View(animal);
        }

        public IActionResult Delete(int id)
        {
            bool deleted = _animalAppService.Delete(id);

            if (deleted)
            {
                return RedirectToAction(nameof(AnimalController.Select));
            }
            else
                return NotFound();
        }
    }
}
