using System.Threading.Tasks;
using BudgetApp.Data.Models;
using BudgetApp.Infrastructure.Web.Filters;
using BudgetApp.Services.Auth;
using BudgetApp.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    [Route("auth")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RegistrationService _registrationService;
        
        public AuthController(SignInManager<User> signInManager, RegistrationService registrationService)
        {
            _signInManager = signInManager;
            _registrationService = registrationService;
        }

        [HttpGet("login")]
        [ImportModelState]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet("register")]
        [ImportModelState]
        public IActionResult Register() => View();

        [HttpPost("login")]
        [ValidateModelState]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
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
        [ValidateModelState]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _registrationService.Register(model);

            return MapToResponse(result, () => RedirectToAction(nameof(ExpensesController.Search), "Expenses"));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}