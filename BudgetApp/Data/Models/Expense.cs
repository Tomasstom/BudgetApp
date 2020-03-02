using System;

namespace BudgetApp.Data.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        
        public User User { get; set; }
        public ExpenseCategory Category { get; set; }
    }
}