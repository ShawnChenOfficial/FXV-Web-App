using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FXV.Data;
using FXV.Models;
using FXV.ViewModels.NewModels;
using FXV_App.CustomizeControllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV.Controllers
{
    public class ActivityController : CustomizeController
    {

        public ActivityController (IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(int testid)
        {
            TempData["Splittable"] = await _dbContext.Test.Where(w => w.Test_ID == testid).Select(s => s.IsSplittable).FirstOrDefaultAsync();
            TempData["TestId"] = testid;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TestActivity">TestActivity is a collection of test activity setup information, which contains base test id, a list of attendee ids, and a list of split test ids</param>
        /// <param name="TestActivity.SplitTestIds">This is a collection of split test ids. This param can be null if there is no Split tests.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RunActivity(ViewModel_TestActivity TestActivity)
        {


            return new OkResult();
        }
    }
}
