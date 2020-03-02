using System.Threading.Tasks;
using BudgetApp.Data.Models;
using BudgetApp.Services.Auth;
using BudgetApp.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RegistrationService _registrationService;
        
        public AuthController(SignInManager<User> signInManager, RegistrationService registrationService)
        {
            _signInManager = signInManager;
            _registrationService = registrationService;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register() => View();

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

            returnUrl = returnUrl ?? Url.Content("~/");

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Nazwa użytkownika lub hasło są niepoprawne.");
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            await _registrationService.Register(model);

            return RedirectToAction(nameof(ExpensesController.List), "Expenses");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}