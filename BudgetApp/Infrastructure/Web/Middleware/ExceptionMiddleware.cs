using System;
using System.Text.Json;
using System.Threading.Tasks;
using BudgetApp.Infrastructure.Web.Extensions;
using Microsoft.AspNetCore.Http;

namespace BudgetApp.Infrastructure.Web.Middleware
{
    public class ExceptionMiddleware 
    { 
        private readonly RequestDelegate _next; 

        public ExceptionMiddleware(RequestDelegate next) 
        { 
            _next = next; 
        } 

        public async Task Invoke(HttpContext context) 
        { 
            try
            {
                await _next(context);
            } 
            catch (Exception ex) 
            { 
                await HandleException(context, ex); 
            } 
        } 

        private static async Task HandleException(HttpContext context, Exception exception) 
        {
            if (context.Request.IsAjaxRequest())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse("Wystąpił błąd."),
                    options));
            }
            else
            {
                context.Response.Redirect("/error"); 
            }
        } 
    } 
}