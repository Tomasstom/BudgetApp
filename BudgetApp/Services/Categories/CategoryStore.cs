using System;
using System.Linq;
using BudgetApp.Common.Identity;
using BudgetApp.Common.Results;
using BudgetApp.Data;
using BudgetApp.Data.Models;
using BudgetApp.ViewModels.Expenses.Input;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Services.Categories
{
    public class CategoryStore
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserContext _currentUser;

        public CategoryStore(ApplicationDbContext db, ICurrentUserContext currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public Result<int> Add(AddCategoryViewModel model)
        {
            var userCategories = _db.ExpenseCategories.Where(c => c.UserId == _currentUser.Id).ToList();

            if (userCategories.Any(c => c.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase)))
                return Result.NotValid("Kategoria o tej nazwie już istnieje.");

            var category = new ExpenseCategory
            {
                Name = model.Name,
                UserId = _currentUser.Id
            };

            _db.ExpenseCategories.Add(category);
            _db.SaveChanges();

            return Result.Ok(category.Id);
        }

        public Result Remove(int categoryId)
        {
            var category = _db.ExpenseCategories
                .Include(c => c.Expenses)
                .FirstOrDefault(e => e.Id == categoryId);
            
            if (category is null)
                return Result.NotValid("Kategoria nie istnieje.");
            
            if (category.UserId != _currentUser.Id)
                return Result.NotAuthorized("Nie możesz usunąć kategorii nienależącej do Ciebie.");
            
            if (category.Expenses.Any())
                return Result.NotValid("Musisz usunąć wydatki przypisane do tej kategorii przed jej usunięciem.");

            _db.ExpenseCategories.Remove(category);
            _db.SaveChanges();
            
            return Result.Ok();
        }
    }
}