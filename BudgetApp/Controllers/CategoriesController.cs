using BudgetApp.Services.Categories;
using BudgetApp.ViewModels.Expenses.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    [Authorize]
    [Route("categories")]
    public class CategoriesController : Controller
    {
        private readonly CategoryStore _categoryStore;
        private readonly CategoryReader _categoryReader;

        public CategoriesController(CategoryStore categoryStore, CategoryReader categoryReader)
        {
            _categoryStore = categoryStore;
            _categoryReader = categoryReader;
        }

        [HttpGet("add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            _ = _categoryStore.Add(model);
            return RedirectToAction(nameof(List));
        }
        
        [HttpGet("")]
        public IActionResult List()
        {
            var expenses = _categoryReader.GetAll();
            return View(expenses);
        }
    }
}