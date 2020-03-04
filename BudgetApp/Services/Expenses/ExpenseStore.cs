using System;
using BudgetApp.Common.Identity;
using BudgetApp.Common.Results;
using BudgetApp.Data;
using BudgetApp.Data.Models;
using BudgetApp.ViewModels.Expenses.Input;

namespace BudgetApp.Services.Expenses
{
    public class ExpenseStore
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserContext _currentUser;
        
        public ExpenseStore(ApplicationDbContext db, ICurrentUserContext currentUserContext)
        {
            _db = db;
            _currentUser = currentUserContext;
        }

        public Result<int> Add(AddExpenseViewModel model)
        {
            var category = _db.ExpenseCategories.Find(model.CategoryId);
            
            if (category is null)
                return Result.NotValid("Kategoria nie istnieje.");
            
            if (category.UserId != _currentUser.Id)
                return Result.NotValid("Ta kategoria nie należy do Ciebie.");
            
            var expense = new Expense
            {
                Name = model.Name,
                Value = model.Value,
                DateTime = model.DateTime ?? DateTime.UtcNow,
                UserId = _currentUser.Id,
                CategoryId = category.Id
            };

            _db.Expenses.Add(expense);
            _db.SaveChanges();

            return Result.Ok(expense.Id);
        }

        public Result Edit(EditExpenseViewModel model)
        {
            var expense = _db.Expenses.Find(model.ExpenseId);
            var category = _db.ExpenseCategories.Find(model.CategoryId);
            
            if (expense is null)
                return Result.NotValid("Wydatek nie został znaleziony.");
            
            if (expense.UserId != _currentUser.Id)
                return Result.NotValid("Ten wydatek nie należy do Ciebie.");
            
            if (category is null)
                return Result.NotValid("Kategoria nie istnieje.");
            
            if (category.UserId != _currentUser.Id)
                return Result.NotValid("Ta kategoria nie należy do Ciebie.");

            expense.Name = model.Name;
            expense.Value = model.Value;
            expense.DateTime = model.DateTime ?? DateTime.UtcNow;
            expense.CategoryId = category.Id;

            _db.SaveChanges();
            
            return Result.Ok();
        }
        
        public Result Remove(int expenseId)
        {
            var expense = _db.Expenses.Find(expenseId);
            
            if (expense is null)
                return Result.NotValid("Wydatek nie został znaleziony.");
            
            if (expense.UserId != _currentUser.Id)
                return Result.NotAuthorized("Ten wydatek nie został dodany przez Ciebie.");
            
            _db.Expenses.Remove(expense);
            _db.SaveChanges();
            
            return Result.Ok();
        }
    }
}