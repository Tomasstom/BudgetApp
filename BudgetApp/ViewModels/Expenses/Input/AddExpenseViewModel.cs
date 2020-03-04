using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetApp.ViewModels.Expenses.Input
{
    public class AddExpenseViewModel
    {
        [Required(ErrorMessage = "Nazwa wydatku nie może pozostać pusta.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        
        [Display(Name = "Wartość")]
        [RegularExpression("\\d+((\\.|,)\\d{2})?", ErrorMessage = "Wartość nie jest poprawna.")]
        public decimal Value { get; set; }
        
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }
        
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime? DateTime { get; set; }
        
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}