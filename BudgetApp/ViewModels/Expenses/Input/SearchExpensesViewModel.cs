using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetApp.ViewModels.Expenses.Input
{
    public class SearchExpensesViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        
        [Display(Name = "Okres czasu")]
        public ExpenseTimeSpan TimeSpan { get; set; }
        
        [Display(Name = "Kategoria")]
        public int? CategoryId { get; set; }
        
        [Display(Name = "Kolejność")]
        public ExpenseOrder Order { get; set; }
        
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}