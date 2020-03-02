using BudgetApp.Services.Categories;
using BudgetApp.Services.Expenses;
using BudgetApp.ViewModels.Expenses.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    [Authorize]
    [Route("expenses")]
    public class ExpensesController : Controller
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
        public IActionResult Add()
        {
            return View(new AddExpenseViewModel
            {
                Categories = _categoryReader.GetSelectList()
            });
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoryReader.GetSelectList();
                return View(model);
            }
            
            _ = _expenseStore.Add(model);
            return RedirectToAction(nameof(List));
        }
        
        [HttpGet("")]
        public IActionResult List()
        {
            var expenses = _expenseReader.GetAll();
            return View(expenses);
        }
    }
}