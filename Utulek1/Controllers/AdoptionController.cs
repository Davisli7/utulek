using Microsoft.AspNetCore.Mvc;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;

namespace Utulek1.Controllers
{
    public class AdoptionController : Controller
    {
        private readonly IAnimalAppService _animalAppService;

        public AdoptionController(IAnimalAppService animalAppService)
        {
            _animalAppService = animalAppService;
        }

        public IActionResult Index()
        {
            // Načteme všechna zvířata (včetně fotek díky tvé úpravě v Service)
            IList<Animal> animals = _animalAppService.Select();
            return View(animals);
        }
    }
}
