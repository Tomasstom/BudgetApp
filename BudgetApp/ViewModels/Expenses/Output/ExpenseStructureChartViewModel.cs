using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.ViewModels.Expenses.Output
{
    public class ExpenseStructureChartViewModel
    {
        [Display(Name = "Okres czasu")]
        public ExpenseTimeSpan TimeSpan { get; set; }
        public IEnumerable<ExpenseStructureItemViewModel> Items { get; set; }
    }
}