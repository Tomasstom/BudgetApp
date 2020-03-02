using System;

namespace BudgetApp.ViewModels.Expenses.Output
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime DateTime { get; set; }
    }
}