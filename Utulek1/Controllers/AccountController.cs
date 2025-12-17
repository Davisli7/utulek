using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Utulek1.Domain.Entities;
using Utulek1.Models;

namespace Utulek1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
        ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // --- PŘIHLÁŠENÍ ---
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // Přihlášení uživatele
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Uživatel '{UserName}' se úspěšně přihlásil.", model.Username);
                    // Pokud existuje ReturnUrl (uživatel chtěl někam jít), vrátíme ho tam, jinak na Home
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Neúspěšný pokus o přihlášení pro uživatele '{UserName}'.", model.Username);
                    ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje.");
                    return View(model);
                }
            }
            return View(model);
        }

        // --- REGISTRACE ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed = true 
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    _logger.LogInformation("Nový uživatel se registroval: '{UserName}' (ID: {UserId}, Email: {Email}).",
                        user.UserName, user.Id, user.Email);
                    // Automaticky přiřadíme roli "Customer"
                    await _userManager.AddToRoleAsync(user, "Customer");

                    // A rovnou ho přihlásíme
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Registrace selhala pro uživatele '{UserName}'. Chyba: {ErrorCode} - {Description}",
                        model.Username, error.Code, error.Description);

                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // --- ODHLÁŠENÍ ---
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name;

            await _signInManager.SignOutAsync();

            _logger.LogInformation("Uživatel '{UserName}' se odhlásil.", userName);

            return RedirectToAction("Index", "Home");
        }

        // --- PŘÍSTUP ZAMÍTNUT ---
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}