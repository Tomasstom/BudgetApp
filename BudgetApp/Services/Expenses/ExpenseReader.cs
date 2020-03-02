using System.Collections.Generic;
using System.Linq;
using BudgetApp.Common.Identity;
using BudgetApp.Data;
using BudgetApp.ViewModels.Expenses.Output;

namespace BudgetApp.Services.Expenses
{
    public class ExpenseReader
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserContext _currentUser;

        public ExpenseReader(ApplicationDbContext db, ICurrentUserContext currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public IEnumerable<ExpenseViewModel> GetAll()
        {
            return _db.Expenses
                .Where(e => e.UserId == _currentUser.Id)
                .OrderByDescending(e => e.DateTime)
                .Select(e => new ExpenseViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category.Name,
                    DateTime = e.DateTime,
                    Value = e.Value
                })
                .ToList();
        }
    }
}