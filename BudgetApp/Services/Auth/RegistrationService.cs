using System.Threading.Tasks;
using BudgetApp.Common.Results;
using BudgetApp.Data;
using BudgetApp.Data.Models;
using BudgetApp.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;

namespace BudgetApp.Services.Auth
{
    public class RegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _db;

        public RegistrationService(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public async Task<Result> Register(RegisterViewModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return Result.NotValid("Użytkownik o podanym adresie email już istnieje.");
            
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
                return Result.NotSpecified("Rejestracja nie powiodła się z nieznanych przyczyn");

            await _signInManager.SignInAsync(user, false, model.Password);
            
            return Result.Ok();
        }
    }
}