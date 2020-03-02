using System;

namespace BudgetApp.Common
{
    public class BudgetAppException : Exception
    {
        public ErrorCode Code { get; }

        public BudgetAppException(ErrorCode code, string message) : base(message)
        {
            Code = code;
        }
    }
}