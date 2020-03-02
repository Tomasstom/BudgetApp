using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetApp.Common;
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

        public async Task Register(RegisterViewModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                throw new BudgetAppException(ErrorCode.NotValid, "Użytkownik o podanym adresie email już istnieje.");
            
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
                throw new BudgetAppException(ErrorCode.Error, "Rejestracja nie powiodła się z nieznanych przyczyn.");

            await AddInitialData(user.Id);

            await _signInManager.SignInAsync(user, false, model.Password);
        }

        private async Task AddInitialData(string userId)
        {
            var categories = new List<ExpenseCategory>
            {
                new ExpenseCategory
                {
                    Name = "Jedzenie",
                    UserId = userId,
                    Expenses = new List<Expense>
                    {
                        new Expense
                        {
                            Name = "Pizza",
                            DateTime = DateTime.UtcNow.AddDays(-1),
                            Value = 40,
                            UserId = userId
                        },
                    }
                },
                new ExpenseCategory
                {
                    Name = "Rozrywka",
                    UserId = userId,
                    Expenses = new List<Expense>
                    {
                        new Expense
                        {
                            Name = "Netflix",
                            DateTime = DateTime.UtcNow.AddDays(-10),
                            Value = 50,
                            UserId = userId
                        },
                    }
                },
                new ExpenseCategory
                {
                    Name = "Komunikacja",
                    UserId = userId,
                    Expenses = new List<Expense>
                    {
                        new Expense
                        {
                            Name = "Bilet MPK",
                            DateTime = DateTime.UtcNow.AddDays(-10),
                            Value = 100,
                            UserId = userId
                        },
                    }
                }
            };

            _db.ExpenseCategories.AddRange(categories);
            await _db.SaveChangesAsync();
        }
    }
}