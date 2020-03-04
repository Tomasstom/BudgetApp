using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetApp.ViewModels.Expenses.Output
{
    public class ExpensePageViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ExpenseViewModel> Expenses { get; set; }
        public ExpenseTimeSpan TimeSpan { get; set; }
        public int? CategoryId { get; set; }
        public ExpenseOrder Order { get; set; }
        
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}