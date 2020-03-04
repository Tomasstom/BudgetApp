using System.Collections.Generic;

namespace BudgetApp.Infrastructure.Web
{
    public class ApiResponse
    {
        public ApiResponse(string message, IReadOnlyDictionary<string, string> data = null)
        {
            Message = message;
            Data = data;
        }

        public string Message { get; }

        public IReadOnlyDictionary<string, string> Data { get; }
    }
}