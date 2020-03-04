using BudgetApp.Infrastructure.Web.Filters;
using BudgetApp.Services.Categories;
using BudgetApp.ViewModels.Expenses.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    [Authorize]
    [Route("categories")]
    public class CategoriesController : BaseController
    {
        private readonly CategoryStore _categoryStore;
        private readonly CategoryReader _categoryReader;

        public CategoriesController(CategoryStore categoryStore, CategoryReader categoryReader)
        {
            _categoryStore = categoryStore;
            _categoryReader = categoryReader;
        }

        [HttpGet("add")]
        [ImportModelState]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("add")]
        [ValidateModelState]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddCategoryViewModel model)
        {
            var result = _categoryStore.Add(model);
            return MapToResponse(result, () => RedirectToAction(nameof(List)));
        }
        
        [HttpGet("")]
        public IActionResult List()
        {
            var expenses = _categoryReader.GetAll();
            return View(expenses);
        }
        
        [HttpDelete("{categoryId}")]
        public IActionResult Remove(int categoryId)
        {
            var result = _categoryStore.Remove(categoryId);

            return MapToResponse(result, NoContent);
        }
    }
}