using Utulek1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Utulek1.Application.Abstraction;

namespace Utulek1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AnimalController : Controller
    {
        IAnimalAppService _animalAppService;

        public AnimalController(IAnimalAppService animalAppService)
        {
            _animalAppService = animalAppService;
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
            return View();
        }

        [HttpPost]
        public IActionResult Create(Animal animal, IEnumerable<IFormFile> uploadedFiles)
        {
            _animalAppService.Create(animal, uploadedFiles);

            return RedirectToAction(nameof(AnimalController.Select));
        }
    }
}
