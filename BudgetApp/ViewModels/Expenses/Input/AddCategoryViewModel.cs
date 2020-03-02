using System.ComponentModel.DataAnnotations;

namespace BudgetApp.ViewModels.Expenses.Input
{
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = "Nazwa kategorii nie może pozostać pusta.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
    }
}