using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXV.Data;
using FXV.JwtManager;
using FXV.Models;
using FXV.ViewModels;
using FXV_App.CustomizeControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class EventMenuController : CustomizeController
    {
        public EventMenuController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult EventMenu(int id)
        {
            var event_builder = _dbContext.Event_Builder.Find(id);
            var eve = _dbContext.Event.Find(id);
            TempData["Event_id"] = id;
            TempData["event_name"] = eve.Name;
            TempData["event_date"] = eve.Date.ToString("dd MMM yyy");
            TempData["event_time"] = eve.Time.ToString("hh:mm tt");
            TempData["event_location"] = eve.Location;
            TempData["combine_name"] = _dbContext.Combine.Find(event_builder.C_ID).Name;
            return View();
        }
    }
}
