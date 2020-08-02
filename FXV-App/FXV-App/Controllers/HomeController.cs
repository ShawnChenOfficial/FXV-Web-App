using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FXV.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FXV.Data;
using FXV.JwtManager;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using FXV.JasonConstructor;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using FXV_App.Controllers;
using FXV_App.CustomizeControllers;

namespace FXV.Controllers
{
    [Authorize]
    public class HomeController : CustomizeController
    {
        public HomeController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Index", "Account");
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetActicitiesSigned(string date)
        {
            var convertedDate = DateTime.ParseExact(date, "ddd MMM dd yyyy HH:mm:ss 'GMT'K", CultureInfo.InvariantCulture);

            var today = DateTime.Now;

            var html = "";

            if (convertedDate.Date < today.Date)
            {
                html = "<h3 class='medium-bold-500 card-title text-center text-white'>Signed Activities</h3>"
                        + "<h5 class='medium-bold-500 card-title text-center text-white'>" + convertedDate.ToString("MM/dd/yyyy") + "</h3>"
                        + "<div class='pl-3 pt-2 mt-2 mr-1 card-shadow'>"
                        + "<div class='row'>"
                        + " <h4 class='col-12 text-center'>You haven't signed to any event.</h4>"
                        + " </div>"
                        + "</div>";
            }
            else
            {
                var assigned_events = _dbContext.Event_Assigned_Attendee.Where(x => x.Id == _claim.Uid);

                var events = _dbContext.Event.Where(x => x.Date.Date == convertedDate.Date && ((x.Date.Date == today.Date) ? TimeSpan.Compare(x.Time.TimeOfDay, today.TimeOfDay) > 0 : true)).ToList();

                var assigned_events_details = events.Where(x => assigned_events.Select(y => y.E_ID).Contains(x.E_ID)).ToList();

                if (assigned_events_details.Count == 0)
                {
                    html = "<h3 class='medium-bold-500 card-title text-center text-white'>Signed Activities</h3>"
                            + "<h5 class='medium-bold-500 card-title text-center text-white'>" + convertedDate.ToString("MM/dd/yyyy") + "</h3>"
                            + "<div class='pl-3 pt-2 mt-2 mr-1 card-shadow'>"
                            + "<div class='row'>"
                            + " <h4 class='col-12 text-center'>You haven't signed to any event.</h4>"
                            + " </div>"
                            + "</div>";
                }
                else
                {
                    html = "<h3 class='medium-bold-500 card-title text-center text-white'>Signed Activities</h3>"
                            + "<h5 class='medium-bold-500 card-title text-center text-white'>" + convertedDate.ToString("MM/dd/yyyy") + "</h3>";
                    foreach (var eve in assigned_events_details)
                    {
                        html += "<div class='pl-3 pt-2 mt-2 mr-1 card-shadow'>"
                            + "<!--subtitle-->"
                            + "<div class='row'>"
                            + " <h3 class='col-9 m-0 content-orange medium-bold-400 font-weight-bold'>" + eve.Name + "</h3>"
                            + " <i class='mt-2 material-icons text-center col-2 content-warning event-signature'>assignment_turned_in</i>"
                            + "</div>"
                            + "<!--date-->"
                            + "<p class='text-white content-font-size medium-bold-400'>" + eve.Date.DayOfWeek + " " + eve.Date.Day + " " + eve.Date.Year + "</p>"
                            + "<!--time & location-->"
                            + "<div class='text-white mt-2 row'>"
                            + " <p class='col-4 content-font-size medium-bold-400'>" + eve.Time.ToString("hh:mm tt") + "</p>"
                            + "  <div class='col-8 p-0'>"
                            + "     <h5>"
                            + "     <span class='material-icons'>room</span>"
                            + "     <sup class='content-font-size medium-bold-400'>" + eve.Location + "</sup>"
                            + "     </h5>"
                            + "  </div></div></div>";
                    }
                }
            }


            return Content(html);
        }


        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetActicitiesUnsigned(string date)
        {
            var convertedDate = DateTime.ParseExact(date, "ddd MMM dd yyyy HH:mm:ss 'GMT'K", CultureInfo.InvariantCulture);

            var today = DateTime.Now;

            var html = "";

            if (convertedDate.Date < today.Date)
            {
                html = "<h3 class='medium-bold-500 card-title text-center text-white'>Unsigned Activities</h3>"
                        + "<h5 class='medium-bold-500 card-title text-center text-white'>" + convertedDate.ToString("MM/dd/yyyy") + "</h3>"
                        + "<div class='pl-3 pt-2 mt-2 mr-1 card-shadow'>"
                        + "<div class='row'>"
                        + " <h4 class='col-12 text-center'>There is no event unsigned.</h4>"
                        + " </div>"
                        + "</div>";
            }
            else
            {
                var assigned_events = _dbContext.Event_Assigned_Attendee.Where(x => x.Id == _claim.Uid);

                var events = _dbContext.Event.Where(x => x.Date.Date == convertedDate.Date && ((x.Date.Date == today.Date) ? TimeSpan.Compare(x.Time.TimeOfDay, today.TimeOfDay) > 0 : true)).ToList();

                var unsigned_events_details = events.Where(x => !assigned_events.Select(z => z.E_ID).Contains(x.E_ID)).ToList(); // issues


                if (unsigned_events_details.Count == 0)
                {
                    html = "<h3 class='medium-bold-500 card-title text-center text-white'>Unsigned Activities</h3>"
                            + "<h5 class='medium-bold-500 card-title text-center text-white'>" + convertedDate.ToString("MM/dd/yyyy") + "</h3>"
                            + "<div class='pl-3 pt-2 mt-2 mr-1 card-shadow'>"
                            + "<div class='row'>"
                            + " <h4 class='col-12 text-center'>There is no event.</h4>"
                            + " </div>"
                            + "</div>";
                }
                else
                {
                    html = "<h3 class='medium-bold-500 card-title text-center text-white'>Unsigned Activities</h3>"
                              + "<h5 class='medium-bold-500 card-title text-center text-white' >" + convertedDate.ToString("MM/dd/yyyy") + "</h5>";

                    foreach (var eve in unsigned_events_details)
                    {
                        html = html + "<div class='pl-3 pt-2 mt-2 mr-1 card-shadow'>"
                            + "<h3 class='m-0 content-orange medium-bold-400 font-weight-bold'>" + eve.Name + "</h3>"
                                 + "<p class='text-white content-font-size medium-bold-400'>" + eve.Date.DayOfWeek + " " + eve.Date.Day + " " + eve.Date.Year + "</p>"
                                  + " <div class='text-white mt-2 row'>"
                                    + " <p class='col-4 content-font-size medium-bold-400'>" + eve.Time.ToString("hh:mm tt") + "</p>"
                                     + "   <div class='col-8 p-0'>"
                                    + "       <h5>"
                                   + "           <span class='material-icons'>room</span>"
                                   + "           <sup class='content-font-size medium-bold-400'>" + eve.Location + "</sup>"
                                   + "      </h5>"
                                    + "  </div>"
                                    + " </div>"
                                    + " <div class='row'>"
                                     + " <p class='col-6 text-white content-font-size medium-bold-400'>" + eve.Description + "  </p>"
                                     + "  <img class='col-6 pb-3' src=\'" + eve.Img_Path + "\' /></div>";

                    }

                    html += " </div>";
                }

            }

            return Content(html);
        }
        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetDashboardIndividualTestData()
        {
            List<IndividualTestData> list = new List<IndividualTestData>();

            IndividualTestData data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "30M Sprint";
            data.Result = 8.32;
            data.Unit = " " + "s";
            data.Top_Result = 12.33;

            list.Add(data);


            data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "100M Sprint";
            data.Result = 23.22;
            data.Unit = " " + "s";
            data.Top_Result = 28.22;

            list.Add(data);


            data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "Vault";
            data.Result = 10.82;
            data.Unit = " " + "m";
            data.Top_Result = 15;

            list.Add(data);


            data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "Rally";
            data.Result = 5;
            data.Unit = " " + "th";
            data.Top_Result = 8;

            list.Add(data);


            data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "Balance Beam";
            data.Result = 8.3;
            data.Unit = " " + "min";
            data.Top_Result = 10;

            list.Add(data);


            data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "Freestyle Swim" + "\n[bold #fff font-size:0.5rem](" + "30min" + ")";
            data.Result = 13;
            data.Unit = " " + "Laps";
            data.Top_Result = 15;

            list.Add(data);


            data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "100M Sprint";
            data.Result = 23.22;
            data.Unit = " " + "s";
            data.Top_Result = 28.22;

            list.Add(data);


            data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "Vault";
            data.Result = 10.82;
            data.Unit = " " + "m";
            data.Top_Result = 15;

            list.Add(data);


            data = new IndividualTestData();

            data.Test_Title = "[bold #fff font-size:1rem]" + "Rally";
            data.Result = 5;
            data.Unit = " " + "th";
            data.Top_Result = 8;

            list.Add(data);


            return Content(JsonConvert.SerializeObject(list), "application/json");
        }
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetDashboardTestBarChartsData(int year)
        {
            var systemDate = DateTime.Now;

            var _testChartsDataList = new TestChartsDataList();

            _testChartsDataList.TestByGenderList = new List<TestByGender>();
            _testChartsDataList.TestByYearsList = new List<TestByYear>();

            var tests = _dbContext.Test.ToList();

            foreach (var x in tests)
            {

                var _testByGender = new TestByGender();
                var _testByYear = new TestByYear();

                _testByGender.Label = x.Name;
                //_testByGender.MaleData = (from t1 in _dbContext.Test_Result.Where(y => y.Test_ID == x.Test_ID)
                //                          join t2 in _dbContext.Users.Where(z => z.Gender == "Male") on t1.Id equals t2.Id
                //                          select t2).GroupBy(z => z.Id).Count();

                _testByGender.MaleData = _dbContext.Test_Result.Where(t => t.Test_ID == x.Test_ID).Include(i => i.AppUser)
                                         .Where(i => i.AppUser.Gender == "Male").Select(w => w.AppUser).GroupBy(i => i.Id).Count();
                //sj tested 25/04/2020, notice adding "Select(w=> w.AppUser)" is of no different for the result, just make it more readable 

                //_testByGender.FemaleData = (from t1 in _dbContext.Test_Result.Where(y => y.Test_ID == x.Test_ID)
                //                            join t2 in _dbContext.Users.Where(z => z.Gender == "Female") on t1.Id equals t2.Id
                //                            select t2).GroupBy(z => z.Id).Count();

                _testByGender.FemaleData = _dbContext.Test_Result.Where(t => t.Test_ID == x.Test_ID).Include(i => i.AppUser)
                                           .Where(i => i.AppUser.Gender == "Female").Select(w => w.AppUser).GroupBy(i => i.Id).Count();
                //sj tested 25/04/2020 notice adding "Select(w=> w.AppUser)" is of no different for the result, just make it more readable

                _testByYear.Label = x.Name;
                _testByYear.DataTwoYearsBefore = _dbContext.Test_Result.Where(y => y.Test_ID == x.Test_ID && y.Date.Year == systemDate.Year - 2)
                                                .GroupBy(z => z.Id).Count();
                _testByYear.DataOneYearBefore = _dbContext.Test_Result.Where(y => y.Test_ID == x.Test_ID && y.Date.Year == systemDate.Year - 1)
                                                .GroupBy(z => z.Id).Count();
                _testByYear.DataCurrentYear = _dbContext.Test_Result.Where(y => y.Test_ID == x.Test_ID && y.Date.Year == systemDate.Year)
                                                .GroupBy(z => z.Id).Count();

                _testChartsDataList.TestByGenderList.Add(_testByGender);
                _testChartsDataList.TestByYearsList.Add(_testByYear);
            }

            return Content(JsonConvert.SerializeObject(_testChartsDataList), "application/json");
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetDashboardMembersData(int year, int month)
        {
            string[] MonthTitle = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            List<MemberByMonth> list = new List<MemberByMonth>();

            year = (month - 12) < 0 ? year - 1 : year;

            for (int i = 0; i <= 12; i++)
            {
                MemberByMonth _memberByMonth = new MemberByMonth();
                _memberByMonth.Label = MonthTitle[month - 1];
                _memberByMonth.Number = _dbContext.Users.Where(x => x.RegisterDate.Year == year && x.RegisterDate.Month == month).Count();

                if (month == 12)
                {
                    year++;
                    month = 1;
                }
                else
                {
                    month++;
                }

                list.Add(_memberByMonth);
            }

            return Content(JsonConvert.SerializeObject(list), "application/json");
        }
    }
}
