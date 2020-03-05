using System;
using System.Collections.Generic;
using System.Linq;
using BudgetApp.Common.Identity;
using BudgetApp.Data;
using BudgetApp.Data.Models;
using BudgetApp.ViewModels.Expenses;
using BudgetApp.ViewModels.Expenses.Input;
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

        public IEnumerable<ExpenseViewModel> Search(SearchExpensesViewModel model)
        {
            var query = _db.Expenses.Where(e => e.UserId == _currentUser.Id);

            if (model.CategoryId.HasValue)
                query = query.Where(e => e.CategoryId == model.CategoryId);

            query = ApplyOrderCriteriion(query, model.Order);
            query = ApplyTimeSpanCriterion(query, model.TimeSpan);

            return query
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
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

        public EditExpenseViewModel GetToEdit(int expenseId)
        {
            return _db.Expenses
                .Where(e => e.Id == expenseId)
                .Select(e => new EditExpenseViewModel
                {
                    ExpenseId = e.Id,
                    Name = e.Name,
                    Value = e.Value.ToString("0.00"),
                    DateTime = e.DateTime,
                    CategoryId = e.CategoryId,
                }).FirstOrDefault();
        }

        public List<ExpenseStructureItemViewModel> GetStructure(ExpenseTimeSpan timeSpan)
        {
            var query = _db.Expenses
                .Where(e => e.UserId == _currentUser.Id);

            query = ApplyTimeSpanCriterion(query, timeSpan);

            var chartItems = query.GroupBy(e => e.Category.Name)
                .Select(g => new ExpenseStructureItemViewModel
                {
                    CategoryName = g.Key,
                    TotalValue = (int) g.Sum(e => (float) e.Value)
                })
                .OrderByDescending(i => i.TotalValue)
                .ToList();

            return chartItems;
        }

        private IQueryable<Expense> ApplyTimeSpanCriterion(IQueryable<Expense> query, ExpenseTimeSpan timeSpan)
        {
            return timeSpan switch
            {
                ExpenseTimeSpan.All => query,
                ExpenseTimeSpan.ThisWeek => query.Where(c => c.DateTime > DateTime.Now.AddDays(-7)),
                ExpenseTimeSpan.ThisMonth => query.Where(c => c.DateTime > DateTime.Now.AddDays(-30)),
                ExpenseTimeSpan.ThisYear => query.Where(c => c.DateTime > DateTime.Now.AddDays(-365)),
                _ => throw new Exception("Nieznany okres wydatków")
            };
        }
        
        private IQueryable<Expense> ApplyOrderCriteriion(IQueryable<Expense> query, ExpenseOrder order)
        {
            return order switch
            {
                ExpenseOrder.ByValue => query.OrderByDescending(e => (float) e.Value),
                ExpenseOrder.ByDateAscending => query.OrderBy(e => e.DateTime),
                ExpenseOrder.ByDateDescending => query.OrderByDescending(e => e.DateTime),
                _ => throw new Exception("Nieznany porządek wydatków")
            };
        }
    }
}