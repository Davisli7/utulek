using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utulek1.Infrastructure;
using Utulek1.Application.Abstraction;
using SystemLog = Utulek1.Domain.Entities.SystemLog;

namespace Utulek1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] 
    public class LogController : Controller
    {
        private readonly ISystemLogAppService _logAppService;

        public LogController(ISystemLogAppService logAppService)
        {
            _logAppService = logAppService;
        }

        public IActionResult Index(string? searchTerm, string? level)
        {

            ViewBag.CurrentSearchTerm = searchTerm;
            ViewBag.CurrentLevel = level;

            var logs = _logAppService.Select(searchTerm, level);
            return View(logs);
        }

        public IActionResult Delete(int id)
        {
            _logAppService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
