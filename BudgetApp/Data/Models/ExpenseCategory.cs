using System.Collections.Generic;

namespace BudgetApp.Data.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}