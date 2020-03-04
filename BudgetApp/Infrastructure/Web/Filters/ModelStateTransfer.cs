using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BudgetApp.Infrastructure.Web.Filters
{
    public abstract class ModelStateTransfer : ActionFilterAttribute
    {
        internal const string Key = nameof(ModelStateTransfer);

        protected void ImportModelState(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as Controller;

            if (!(controller?.TempData[Key] is string serializedModelState)) return;

            if (filterContext.Result is ViewResult)
            {
                var modelState = ModelStateHelper.DeserialiseModelState(serializedModelState);
                filterContext.ModelState.Merge(modelState);
            }
            else
            {
                controller.TempData.Remove(Key);
            }
        }

        protected void ExportModelState(ActionExecutingContext filterContext)
        {
            if (!(filterContext.Controller is Controller controller) || filterContext.ModelState == null) return;

            controller.TempData[Key] = ModelStateHelper.SerialiseModelState(filterContext.ModelState);
        }
    }
}