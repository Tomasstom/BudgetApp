using System.ComponentModel.DataAnnotations;

namespace BudgetApp.ViewModels.Expenses
{
    public enum ExpenseOrder
    {
        [Display(Name = "Od najnowszych")]
        ByDateDescending,
        [Display(Name = "Od najstarszych")]
        ByDateAscending,
        [Display(Name = "Według wartości")]
        ByValue
    }
}