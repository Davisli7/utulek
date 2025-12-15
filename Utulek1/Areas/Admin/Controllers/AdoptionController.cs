using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Enums;

namespace Utulek1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Manager")] // DŮLEŽITÉ: Zabezpečení
    public class AdoptionController : Controller
    {
        private readonly IAdoptionAppService _adoptionAppService;

        public AdoptionController(IAdoptionAppService adoptionAppService)
        {
            _adoptionAppService = adoptionAppService;
        }

        
        public IActionResult Index(string? searchEmail, AdoptionRequestStatus? statusFilter)
        {
            // Uložíme filtry do ViewBag pro zachování ve formuláři
            ViewBag.CurrentSearchEmail = searchEmail;
            ViewBag.CurrentStatusFilter = statusFilter;

            // Zavoláme službu s filtry
            var requests = _adoptionAppService.Select(searchEmail, statusFilter);

            return View(requests);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int requestId, AdoptionRequestStatus status)
        {
            // Zavoláme službu, která změní status žádosti A TAKÉ status zvířete
            _adoptionAppService.UpdateStatus(requestId, status);

            return RedirectToAction(nameof(Index));
        }
    }
}
