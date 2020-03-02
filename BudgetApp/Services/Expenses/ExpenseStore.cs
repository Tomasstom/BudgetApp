using System;
using BudgetApp.Common;
using BudgetApp.Common.Identity;
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

        public int Add(AddExpenseViewModel model)
        {
            var category = _db.ExpenseCategories.Find(model.CategoryId);
            
            if (category is null)
                throw new BudgetAppException(ErrorCode.NotValid, "Kategoria nie istnieje.");
            
            if (category.UserId != _currentUser.Id)
                throw new BudgetAppException(ErrorCode.NotValid, "Ta kategoria nie należy do Ciebie.");
            
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

            return expense.Id;
        }

        public void Remove(int expenseId)
        {
            var expense = _db.Expenses.Find(expenseId);
            
            if (expense is null)
                throw new BudgetAppException(ErrorCode.NotValid, "Wydatek nie został znaleziony.");
            
            if (expense.UserId != _currentUser.Id)
                throw new BudgetAppException(ErrorCode.Forbidden, "Ten wydatek nie został dodany przez Ciebie.");
            
            _db.Expenses.Remove(expense);
            _db.SaveChanges();
        }
    }
}