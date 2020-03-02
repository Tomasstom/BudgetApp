using System;
using System.Linq;
using BudgetApp.Common;
using BudgetApp.Common.Identity;
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

        public int Add(AddCategoryViewModel model)
        {
            var userCategories = _db.ExpenseCategories.Where(c => c.UserId == _currentUser.Id).ToList();

            if (userCategories.Any(c => c.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase)))
                throw new BudgetAppException(ErrorCode.NotValid, "Kategoria o tej nazwie już istnieje.");

            var category = new ExpenseCategory
            {
                Name = model.Name,
                UserId = _currentUser.Id
            };

            _db.ExpenseCategories.Add(category);
            _db.SaveChanges();

            return category.Id;
        }

        public void Remove(int categoryId)
        {
            var category = _db.ExpenseCategories
                .Include(c => c.Expenses)
                .FirstOrDefault(e => e.Id == categoryId);
            
            if (category is null)
                throw new BudgetAppException(ErrorCode.NotValid, "Kategoria nie istnieje.");
            
            if (category.UserId != _currentUser.Id)
                throw new BudgetAppException(ErrorCode.Forbidden, "Nie możesz usunąć kategorii nienależącej do Ciebie.");
            
            if (category.Expenses.Any())
                throw new BudgetAppException(ErrorCode.NotValid, "Musisz usunąć wydatki przypisane do tej kategorii przed jej usunięciem.");

            _db.ExpenseCategories.Remove(category);
            _db.SaveChanges();
        }
    }
}