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
        public IActionResult Create(Animal animal)
        {
            if (ModelState.IsValid)
            {
                _animalAppService.Create(animal);

                return RedirectToAction(nameof(AnimalController.Select));
            }

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
