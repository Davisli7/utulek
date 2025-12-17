using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utulek1.Infrastructure;
using SystemLog = Utulek1.Domain.Entities.SystemLog;

namespace Utulek1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Přístup jen pro Admina
    public class LogController : Controller
    {
        private readonly UtulekDbContext _context;

        public LogController(UtulekDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? levelFilter)
        {
            var query = _context.SystemLogs.AsQueryable();

            // Filtrace podle Levelu (Info, Error...)
            if (!string.IsNullOrEmpty(levelFilter) && levelFilter != "All")
            {
                query = query.Where(l => l.Level == levelFilter);
            }

            // Seřadíme od nejnovějších a vezmeme posledních 100
            var logs = await query.OrderByDescending(l => l.TimeStamp)
                                  .Take(100)
                                  .ToListAsync();

            ViewBag.CurrentLevelFilter = levelFilter;
            return View(logs);
        }
    }
}
