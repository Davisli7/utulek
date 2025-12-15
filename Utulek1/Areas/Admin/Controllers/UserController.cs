using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;

namespace Utulek1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] 
    public class UserController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly UserManager<User> _userManager;

        public UserController(IUserAppService userAppService, UserManager<User> userManager)
        {
            _userAppService = userAppService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userAppService.SelectAll();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Získáme ID přihlášeného admina
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction(nameof(Index));

            bool success = await _userAppService.Delete(id, currentUser.Id);

            if (success)
                TempData["SuccessMessage"] = "Uživatel byl úspěšně smazán.";
            else
                TempData["ErrorMessage"] = "Uživatele nelze smazat (nelze smazat sebe nebo uživatel neexistuje).";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction(nameof(Index));

            bool success = await _userAppService.ChangeRole(id, role, currentUser.Id);

            if (success)
                TempData["SuccessMessage"] = $"Role uživatele byla změněna na {role}.";
            else
                TempData["ErrorMessage"] = "Roli nelze změnit (nelze měnit vlastní roli).";

            return RedirectToAction(nameof(Index));
        }
    }
}
