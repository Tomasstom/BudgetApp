using System;
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
        public decimal Value { get; set; }
        
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }
        
        [Display(Name = "Data")]
        public DateTime? DateTime { get; set; }
        
        public SelectList Categories { get; set; }
    }
}