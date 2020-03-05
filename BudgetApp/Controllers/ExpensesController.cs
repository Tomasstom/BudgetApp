using BudgetApp.Infrastructure.Web.Filters;
using BudgetApp.Services.Categories;
using BudgetApp.Services.Expenses;
using BudgetApp.ViewModels.Expenses;
using BudgetApp.ViewModels.Expenses.Input;
using BudgetApp.ViewModels.Expenses.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    [Authorize]
    [Route("expenses")]
    public class ExpensesController : BaseController
    {
        private readonly ExpenseStore _expenseStore;
        private readonly ExpenseReader _expenseReader;
        private readonly CategoryReader _categoryReader;
        
        public ExpensesController(ExpenseStore expenseStore, ExpenseReader expenseReader, CategoryReader categoryReader)
        {
            _expenseStore = expenseStore;
            _expenseReader = expenseReader;
            _categoryReader = categoryReader;
        }

        [HttpGet("add")]
        [ImportModelState]
        public IActionResult Add()
        {
            return View(new AddExpenseViewModel
            {
                Categories = _categoryReader.GetSelectList()
            });
        }

        [HttpPost("add")]
        [ValidateModelState]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddExpenseViewModel model)
        {
            _ = _expenseStore.Add(model);
            return RedirectToAction(nameof(Search));
        }
        
        [HttpGet("{expenseId}/edit")]
        [ImportModelState]
        public IActionResult Edit(int expenseId)
        {
            var model = _expenseReader.GetToEdit(expenseId);
            model.Categories = _categoryReader.GetSelectList();

            return View(model);
        }

        [HttpPost("{expenseId}/edit")]
        [ValidateModelState]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditExpenseViewModel model)
        {
            var result = _expenseStore.Edit(model);
            return MapToResponse(result, () => RedirectToAction(nameof(Search)));
        }
        
        [HttpGet("")]
        public IActionResult Search(SearchExpensesViewModel model)
        {
            var expenses = _expenseReader.Search(model);
            var categories = _categoryReader.GetSelectList();

            var page = new ExpensePageViewModel
            {
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
                CategoryId = model.CategoryId,
                Order = model.Order,
                TimeSpan = model.TimeSpan,
                Expenses = expenses,
                Categories = categories
            };
            
            return View(page);
        }

        [HttpDelete("{expenseId}")]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int expenseId)
        {
            var result = _expenseStore.Remove(expenseId);

            return MapToResponse(result, NoContent);
        }

        [HttpGet("structure")]
        public IActionResult Structure(ExpenseTimeSpan timeSpan)
        {
            var items = _expenseReader.GetStructure(timeSpan);
            
            return View(new ExpenseStructureChartViewModel
            {
                TimeSpan = timeSpan,
                Items = items
            });
        }
    }
}