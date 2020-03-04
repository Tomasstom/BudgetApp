using System;
using BudgetApp.Common.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;
using BudgetApp.Data.Models;
using BudgetApp.Infrastructure.Web.Binders;
using BudgetApp.Infrastructure.Web.Middleware;
using BudgetApp.Services.Auth;
using BudgetApp.Services.Categories;
using BudgetApp.Services.Expenses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BudgetApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=app.db"));
            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
           {
               // Password settings.
               options.Password.RequireDigit = false;
               options.Password.RequireLowercase = false;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequireUppercase = false;
               options.Password.RequiredLength = 6;
               options.Password.RequiredUniqueChars = 0;

               // Lockout settings.
               options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
               options.Lockout.MaxFailedAccessAttempts = 5;
               options.Lockout.AllowedForNewUsers = true;
           });

           services.ConfigureApplicationCookie(options =>
           {
               // Cookie settings
               options.Cookie.HttpOnly = true;
               options.ExpireTimeSpan = TimeSpan.FromMinutes(7*24*60*60);

               options.LoginPath = "/auth/login";
               options.AccessDeniedPath = "/auth/access_denied";
               options.SlidingExpiration = true;
           });
           
           services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
           
           services.AddScoped<ICurrentUserContext, CurrentUserContext>();
           services.AddScoped<ExpenseStore>();
           services.AddScoped<CategoryStore>();
           services.AddScoped<ExpenseReader>();
           services.AddScoped<CategoryReader>();
           services.AddScoped<RegistrationService>();

           services.AddSession();
            
           services.AddControllersWithViews(config =>
               {
                   config.ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());
               })
               .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDatabaseErrorPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseCookiePolicy();
            app.UseSession();
            
            app.UseMiddleware(typeof(ExceptionMiddleware));
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
