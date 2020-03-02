using System;
using System.Threading.Tasks;
using BudgetApp.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BudgetApp.Infrastructure.Web
{
    public class ExceptionMiddleware 
    { 
        private readonly RequestDelegate next; 

        public ExceptionMiddleware(RequestDelegate next) 
        { 
            this.next = next; 
        } 

        public async Task Invoke(HttpContext context) 
        { 
            try
            {
                await next(context);
            } 
            catch (Exception ex) 
            { 
                HandleException(context, ex); 
            } 
        } 

        private static void HandleException(HttpContext context, Exception exception) 
        {
            var factory = context.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
            var tempData = factory.GetTempData(context);
            
            if (exception is BudgetAppException budgetAppException)
                tempData.Add("Error", budgetAppException.Message);
            else
                tempData.Add("Error", "Wystąpił błąd.");
            
            context.Response.Redirect("/home/error"); 
        } 
    } 
}