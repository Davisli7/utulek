using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Enums;

namespace Utulek1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Manager")] 
    public class AdoptionController : Controller
    {
        private readonly IAdoptionAppService _adoptionAppService;

        public AdoptionController(IAdoptionAppService adoptionAppService)
        {
            _adoptionAppService = adoptionAppService;
        }

        
        public IActionResult Index(string? searchEmail, AdoptionRequestStatus? statusFilter)
        {
            ViewBag.CurrentSearchEmail = searchEmail;
            ViewBag.CurrentStatusFilter = statusFilter;

            var requests = _adoptionAppService.Select(searchEmail, statusFilter);

            return View(requests);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int requestId, AdoptionRequestStatus status)
        {
            _adoptionAppService.UpdateStatus(requestId, status);

            return RedirectToAction(nameof(Index));
        }
    }
}
