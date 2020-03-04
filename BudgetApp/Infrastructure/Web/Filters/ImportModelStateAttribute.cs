using Microsoft.AspNetCore.Mvc.Filters;

namespace BudgetApp.Infrastructure.Web.Filters
{
    public class ImportModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ImportModelState(filterContext);

            base.OnActionExecuted(filterContext);
        }
    }
}