using System.Collections.Generic;
using System.Linq;
using BudgetApp.Common.Identity;
using BudgetApp.Data;
using BudgetApp.ViewModels.Expenses.Output;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetApp.Services.Categories
{
    public class CategoryReader
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserContext _currentUser;

        public CategoryReader(ApplicationDbContext db, ICurrentUserContext currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public IEnumerable<CategoryViewModel> GetAll()
        {
            return _db.ExpenseCategories
                .Where(c => c.UserId == _currentUser.Id)
                .OrderBy(c => c.Name)
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

        public SelectList GetSelectList()
        {
            return new SelectList(GetAll(), nameof(CategoryViewModel.Id), nameof(CategoryViewModel.Name));
        }
    }
}