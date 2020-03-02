using BudgetApp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BudgetApp.Common.Identity
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string Id =>
            _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

        public string UserName =>
            _userManager.GetUserName(_httpContextAccessor.HttpContext.User);
    }
}