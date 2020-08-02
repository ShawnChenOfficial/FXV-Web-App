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
    public class SharedToolsController : CustomizeController
    {
        public SharedToolsController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string SearchUsers(string fullName, string gender)
        {
            fullName = fullName.Replace(" ", "");

            var users = _dbContext.Users.Where(x => (x.FirstName.ToLower() + x.LastName.ToLower()).Contains(fullName.ToLower()));

            if (gender != "" && gender != null)
            {
                users = users.Where(w => w.Gender == gender);
            }

            return JsonConvert.SerializeObject(users.ToList());
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string SearchAttendees(string fullName, int e_id)
        {
            fullName = fullName.Replace(" ", "");

            var users = _dbContext.Event_Assigned_Attendee.Include(inc => inc.AppUser).Where(w => w.E_ID == e_id).Select(s => s.AppUser)
                .Where(ww => (ww.FirstName.ToLower() + ww.LastName.ToLower()).Contains(fullName.ToLower())).ToList();

            return JsonConvert.SerializeObject(users);
        }
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string SearchOrgs(string fullName)
        {
            fullName = fullName.Replace(" ", "");

            var orgs = _dbContext.Organization.Where(x => (x.Name.ToLower()).Contains(fullName.ToLower()));

            return JsonConvert.SerializeObject(orgs.ToList());
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string SearchTeams(string fullName)
        {
            fullName = fullName.Replace(" ", "");

            var teams = _dbContext.Team.Where(x => (x.Name.ToLower()).Contains(fullName.ToLower()));

            return JsonConvert.SerializeObject(teams.ToList());
        }
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string SearchTests(string fullName, string category, int e_id, string gender)
        {
            fullName = fullName.Replace(" ", "");
            List<Test> tests = new List<Test>();

            if (category != null && category != "" && e_id == 0)
            {
                tests = _dbContext.Test.Include(inc => inc.Test_Category)
                    .Where( w=>w.Gender == gender &&
                    w.Test_Category.Category == category
                    && w.Name.ToLower().Contains(fullName.ToLower())
                    ).ToList();
            }
            else if (category != null && category != "" && e_id != 0)
            {
                tests = _dbContext.Combine_Builder.Include(inc => inc.Test).Include(iinc => iinc.Test.Test_Category)
                    .Where(
                    w => w.C_ID == _dbContext.Event_Builder.Include(inc => inc.Combine).Where(ww => ww.E_ID == e_id).Select(s => s.Combine.C_ID).FirstOrDefault()
                    && w.Test.Test_Category.Category == category
                    && w.Test.Name.ToLower().Contains(fullName.ToLower())
                    ).Select(s=>s.Test).ToList();
            }
            else
            {
                tests = _dbContext.Test.Where(x => x.Name.ToLower().Contains(fullName.ToLower())).ToList();
            }

            return JsonConvert.SerializeObject(tests);
        }
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string SearchCombines(string fullName)
        {
            fullName = fullName.Replace(" ", "");

            var combines = _dbContext.Combine.Where(x => (x.Name.ToLower()).Contains(fullName.ToLower()));

            return JsonConvert.SerializeObject(combines.ToList());
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string SearchCategory(string fullName)
        {
            fullName = fullName.Replace(" ", "");

            var categories = _dbContext.Test_Category.Where(x => (x.Category.ToLower()).Contains(fullName.ToLower()));

            return JsonConvert.SerializeObject(categories.ToList());
        }
    }
}
