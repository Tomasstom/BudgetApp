using System;
using System.Collections.Generic;
using BudgetApp.Common.Results;
using BudgetApp.Infrastructure.Web;
using BudgetApp.Infrastructure.Web.Extensions;
using BudgetApp.Infrastructure.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult MapToResponse(Result result, Func<IActionResult> onSuccess)
        {
            return result.Status switch
            {
                ResultStatus.Ok => onSuccess(),
                ResultStatus.NotValid => HandleNotValidError(result.Message, result.Details),
                ResultStatus.NotAuthorized => HandleNotAuthorizedError(result.Message),
                ResultStatus.NotFound => HandleNotFoundError(result.Message),
                _ => HandleNotSpecifiedError(result.Message)
            };
        }
        
        protected IActionResult MapToResponse<T>(Result<T> result, Func<IActionResult> onSuccess)
        {
            return result.Status switch
            {
                ResultStatus.Ok => onSuccess(),
                ResultStatus.NotValid => HandleNotValidError(result.Message, result.Details),
                ResultStatus.NotAuthorized => HandleNotAuthorizedError(result.Message),
                ResultStatus.NotFound => HandleNotFoundError(result.Message),
                _ => HandleNotSpecifiedError(result.Message)
            };
        }
        
        private IActionResult HandleNotValidError(string message, IReadOnlyDictionary<string, string> details)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                Response.StatusCode = 400;
                return new JsonResult(new ApiResponse(message, details));
            }

            if (details != null)
            {
                foreach (var key in details.Keys)
                {
                    ModelState.AddModelError(key, details[key]);
                }
            }

            if (message != null)
            {
                ModelState.AddModelError(string.Empty, message);
            }

            TempData[ModelStateTransfer.Key] = ModelStateHelper.SerialiseModelState(ModelState);
            return RedirectToAction();
        }

        private IActionResult HandleNotAuthorizedError(string message)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                Response.StatusCode = 403;
                return new JsonResult(new ApiResponse(message));
            }

            TempData["Error"] = message;
            return Forbid();
        }

        private IActionResult HandleNotFoundError(string message)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                Response.StatusCode = 404;
                return new JsonResult(new ApiResponse(message));
            }

            Response.StatusCode = 404;
            TempData["Error"] = message;
            return RedirectToAction("Error", "Home");
        }

        private IActionResult HandleNotSpecifiedError(string message)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                Response.StatusCode = 500;
                return new JsonResult(new ApiResponse(message));
            }

            Response.StatusCode = 500;
            TempData["Error"] = message;
            return RedirectToAction("Error", "Home");
        }
    }
}