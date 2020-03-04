using BudgetApp.Infrastructure.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BudgetApp.Infrastructure.Web.Filters
{
    public class ValidateModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult(filterContext.ModelState);
                }
                else
                {
                    ExportModelState(filterContext);

                    filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}