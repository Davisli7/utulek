using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure; 
using Utulek1.Domain.Validation;

namespace Utulek1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Manager")]
    public class AnimalController : Controller
    {
        private readonly IAnimalAppService _animalAppService;

        public AnimalController(IAnimalAppService animalAppService)
        {
            _animalAppService = animalAppService;
        }

        public IActionResult Select(string? searchName, int? speciesId, string? status)
        {
            ViewBag.CurrentSearchName = searchName;
            ViewBag.CurrentSpeciesId = speciesId;
            ViewBag.CurrentStatus = status;


            var speciesList = _animalAppService.SelectSpecies();
            ViewBag.SpeciesList = new SelectList(speciesList, "SpeciesID", "Name", speciesId);

            IList<Animal> animals = _animalAppService.Select(searchName, speciesId, status);

            return View(animals);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SpeciesList = new SelectList(_animalAppService.SelectSpecies(), "SpeciesID", "Name");
            ViewBag.BreedList = new SelectList(_animalAppService.SelectBreeds(), "BreedID", "Name");

            ViewBag.AllBreeds = _animalAppService.SelectBreeds()
                .Select(b => new
                {
                    breedID = b.BreedID,   // Malá písmena, ať to ladí s JavaScriptem
                    name = b.Name,
                    speciesID = b.SpeciesID
                })
                .ToList();

            return View();
        }


        [HttpPost]
        public IActionResult Create(Animal animal,
            [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".webp" })]
            [MaxFileSize(10)]
            IEnumerable<IFormFile> uploadedFiles)
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

            ViewBag.SpeciesList = new SelectList(_animalAppService.SelectSpecies(), "SpeciesID", "Name");
            ViewBag.BreedList = new SelectList(_animalAppService.SelectBreeds(), "BreedID", "Name");

            return View(animal);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Animal? animal = _animalAppService.Select(id);

            if (animal == null)
            {
                return NotFound();
            }

            ViewBag.SpeciesList = new SelectList(_animalAppService.SelectSpecies(), "SpeciesID", "Name", animal.SpeciesID);
            ViewBag.BreedList = new SelectList(_animalAppService.SelectBreeds(), "BreedID", "Name", animal.BreedID);

            return View(animal);
        }

        [HttpPost]
        public IActionResult Edit(Animal animal,
            [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".webp" })]
            [MaxFileSize(10)]
            IEnumerable<IFormFile> uploadedFiles)
        {
            ModelState.Remove(nameof(Animal.Breed));
            ModelState.Remove(nameof(Animal.Species));
            ModelState.Remove(nameof(Animal.Photos));
            ModelState.Remove(nameof(Animal.AdoptionRequests));

            if (ModelState.IsValid)
            {
                _animalAppService.Update(animal, uploadedFiles);
                return RedirectToAction(nameof(Select));
            }

            var originalAnimal = _animalAppService.Select(animal.AnimalID);
            if (originalAnimal != null)
            {
                animal.Photos = originalAnimal.Photos;
            }

            ViewBag.SpeciesList = new SelectList(_animalAppService.SelectSpecies(), "SpeciesID", "Name", animal.SpeciesID);
            ViewBag.BreedList = new SelectList(_animalAppService.SelectBreeds(), "BreedID", "Name", animal.BreedID);

            return View(animal);
        }

        [HttpGet]
        public IActionResult DeletePhoto(int photoId, int animalId)
        {
            _animalAppService.DeletePhoto(photoId);

            return RedirectToAction(nameof(Edit), new { id = animalId });
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
