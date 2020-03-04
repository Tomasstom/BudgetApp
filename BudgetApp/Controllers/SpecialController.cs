using System;
using System.Linq;
using BudgetApp.Data;
using BudgetApp.Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers
{
    public class SpecialController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public SpecialController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("/database/seed")]
        public IActionResult Seed()
        {
            try
            {
                Seeder.Seed(_db);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return new JsonResult(new ApiResponse(ex.Message));
            }
            
            return NoContent();
        }
    }
}