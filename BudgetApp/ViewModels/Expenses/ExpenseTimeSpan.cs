using System.ComponentModel.DataAnnotations;

namespace BudgetApp.ViewModels.Expenses
{
    public enum ExpenseTimeSpan
    {
        [Display(Name = "Od początku")]
        All = 0,
        [Display(Name = "Ten tydzień")]
        ThisWeek = 1,
        [Display(Name = "Ten miesiąc")]
        ThisMonth = 2,
        [Display(Name = "Ten rok")]
        ThisYear = 3,
    }
}