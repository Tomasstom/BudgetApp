using System;
using BudgetApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BudgetApp.Data
{
    public static class Seeder
    {
        private static Random _random = new Random();

        public static void Seed(ApplicationDbContext db)
        {
            var hasher = new PasswordHasher<User>();
            
            var user = new User
            {
                UserName = "test@test.com",
                NormalizedUserName = "TEST@TEST.COM",
                Email = "test@test.com",
                NormalizedEmail = "TEST@TEST.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            user.PasswordHash = hasher.HashPassword(user, "Haslo1");
            db.Users.Add(user);

            var categories = new ExpenseCategory[]
            {
                new ExpenseCategory {Name = "Jedzenie", UserId = user.Id},
                new ExpenseCategory {Name = "Rozrywka", UserId = user.Id},
                new ExpenseCategory {Name = "Komunikacja", UserId = user.Id},
                new ExpenseCategory {Name = "Ubrania", UserId = user.Id},
                new ExpenseCategory {Name = "Leki", UserId = user.Id},
            };
            
            db.ExpenseCategories.AddRange(categories);
            db.SaveChanges();

            for (var i = 0; i < 10; i++)
            {
                db.Expenses.AddRange(new Expense[]
                {
                    CreateFakeExpense("Pizza", 30, i, categories[0].Id, user.Id),
                    CreateFakeExpense("Kebab", 15, i, categories[0].Id, user.Id),
                    CreateFakeExpense("McDonald's", 15, i, categories[0].Id, user.Id),
                    CreateFakeExpense("KFC", 15, i, categories[0].Id, user.Id),
                    CreateFakeExpense("Retauracja", 50, i, categories[0].Id, user.Id),
                    CreateFakeExpense("Netflix", 50, i, categories[1].Id, user.Id),
                    CreateFakeExpense("HBO GO", 50, i, categories[1].Id, user.Id),
                    CreateFakeExpense("Bilet MPK", 60, i, categories[2].Id, user.Id),
                    CreateFakeExpense("Bluza", 80, i, categories[3].Id, user.Id),
                    CreateFakeExpense("Spodnie", 80, i, categories[3].Id, user.Id),
                });
            }

            db.SaveChanges();
        }
        
        private static Expense CreateFakeExpense(string name,  decimal averageValue, int months, int categoryId, string userId)
        {
            return new Expense
            {
                Name = name,
                Value = averageValue + _random.Next(-5, 5),
                DateTime = DateTime.UtcNow.AddMonths(-months).AddDays(-_random.Next(30)),
                CategoryId = categoryId,
                UserId = userId,
            };
        }
    }
}