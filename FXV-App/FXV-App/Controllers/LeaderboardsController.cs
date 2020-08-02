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
    public class LeaderboardsController : CustomizeController
    {

        public LeaderboardsController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Leaderboards()
        {
            return View();
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardTests(string gender)
        {
            List<Test> tests = new List<Test>();

            if (!gender.Equals("0"))
            {
                tests = _dbContext.Test.Where(x => x.Gender == gender).ToList();
            }
            else
            {
                tests = _dbContext.Test.ToList();
            }

            return Content( JsonConvert.SerializeObject(tests), "application/json");
        }
        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardTestTeams(string gender)
        {
            List<Team> teams = new List<Team>();

            teams = _dbContext.Team.ToList();

            return Content(JsonConvert.SerializeObject(teams), "application/json");
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardTestResults(int testid, string gender, int? sportid, int? teamid)
        {
            List<Leaderboard_TestResultsList> leaderboard_TestResultsLists = new List<Leaderboard_TestResultsList>();

            List<Test_Result> test_results = _dbContext.Test_Result.Where(x => x.Test_ID == testid && x.Point != 0)
                                                                    .Include(inc => inc.AppUser)
                                                                    .Include(iinc => iinc.Test)
                                                                    .GroupBy(y => y.Id)
                                                                    .Select(b => b.Max(
                                                                        z =>
                                                                    new Test_Result
                                                                    {
                                                                        Id = z.Id,
                                                                        Point = z.Point,
                                                                        Result = z.Result,
                                                                        AppUser = z.AppUser,
                                                                        Test = z.Test
                                                                    })).ToList();

            if (teamid != 0)
            {
                leaderboard_TestResultsLists = test_results.Where(ww =>
                                                            _dbContext.Team_Membership.Where(w => w.Team_ID == teamid)
                                                            .Include(inc => inc.AppUser)
                                                            .Select(s => s.AppUser.Id).Contains(ww.AppUser.Id)
                                                            )
                                                            .Select(ss => new Leaderboard_TestResultsList
                                                            {
                                                                Point = ss.Point,
                                                                Result = ss.Result.ToString() + " " + ss.Test.Unit,
                                                                Runner_Name = ss.AppUser.FirstName + " " + ss.AppUser.LastName
                                                            }).ToList();
            }
            else
            {
                leaderboard_TestResultsLists = test_results.Select(s => new Leaderboard_TestResultsList
                {
                    Point = s.Point,
                    Result = s.Result.ToString() + " " + s.Test.Unit,
                    Runner_Name = s.AppUser.FirstName + " " + s.AppUser.LastName
                }).ToList();
            }

            leaderboard_TestResultsLists.Sort();

            ViewData["List"] = leaderboard_TestResultsLists;

            return View();
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardCombines(string gender)
        {
            List<Combine> tests = new List<Combine>();

            if (!gender.Equals("0"))
            {
                tests = _dbContext.Combine.Where(x => x.Gender == gender).ToList();
            }
            else
            {
                tests = _dbContext.Combine.ToList();
            }

            return Content(JsonConvert.SerializeObject(tests), "application/json");
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardCombineTeams()
        {
            List<Team> teams = _dbContext.Team.ToList();

            return Content(JsonConvert.SerializeObject(teams), "application/json");
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardCombineResults(int combineid, string gender, int? sportid, int teamid)
        {
            List<Leaderboard_CombineResultsList> leaderboard_CombineResultsLists = new List<Leaderboard_CombineResultsList>();

            List<Combine_Result> combine_result = new List<Combine_Result>();

            combine_result = _dbContext.Combine_Result.Where(x => x.C_ID == combineid && x.Point != 0).Include(ww => ww.AppUser)
                .GroupBy(y => y.Id)
                .Select(b => b.Max(
                    z =>
                new Combine_Result
                {
                    Id = z.Id,
                    Point = z.Point,
                    AppUser = z.AppUser
                })).ToList();


            if (teamid != 0)
            {
                leaderboard_CombineResultsLists = combine_result.Where(ww => _dbContext.Team_Membership.Where(w => w.Team_ID == teamid)
                                                            .Include(inc => inc.AppUser.Id)
                                                            .Select(s => s.AppUser.Id).Contains(ww.AppUser.Id))
                                                            .Select(x => new Leaderboard_CombineResultsList
                                                            {
                                                                Point = x.Point,
                                                                Runner_Name = x.AppUser.FirstName + " " + x.AppUser.LastName
                                                            }).ToList();
            }
            else
            {
                leaderboard_CombineResultsLists = combine_result.Select(x => new Leaderboard_CombineResultsList
                {
                    Point = x.Point,
                    Runner_Name = x.AppUser.FirstName + " " + x.AppUser.LastName
                }).ToList();
            }

            leaderboard_CombineResultsLists.Sort();

            ViewData["List"] = leaderboard_CombineResultsLists;

            return View();
        }


        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardEvents()
        {
            List<Event> events = new List<Event>();

            events = _dbContext.Event.ToList();

            return Content(JsonConvert.SerializeObject(events), "application/json");
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardEventTeams()
        {

            List<Team> teams = _dbContext.Team.ToList();

            return Content(JsonConvert.SerializeObject(teams), "application/json");
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetLeaderboardEventResults(int eventid, string gender, int? sportid, int teamid)
        {
            List<Leaderboard_EventResultsList> leaderboard_EventResultsLists = new List<Leaderboard_EventResultsList>();

            List<Event_Result> event_result = new List<Event_Result>();

            event_result = _dbContext.Event_Result.Where(x => x.E_ID == eventid && x.Final_Point != 0).Include(ww => ww.AppUser)
                .GroupBy(y => y.Id)
                .Select(b => b.Max(
                    z =>
                new Event_Result
                {
                    Id = z.Id,
                    Final_Point = z.Final_Point,
                    AppUser = z.AppUser
                })).ToList();


            if (teamid != 0)
            {

                leaderboard_EventResultsLists = event_result.Where(ww => _dbContext.Team_Membership.Where(w => w.Team_ID == teamid)
                                                            .Include(inc => inc.AppUser.Id)
                                                            .Select(s => s.AppUser.Id).Contains(ww.AppUser.Id))
                                                            .Select(x => new Leaderboard_EventResultsList
                                                            {
                                                                Point = x.Final_Point,
                                                                Runner_Name = x.AppUser.FirstName + " " + x.AppUser.LastName
                                                            }).ToList();
            }
            else
            {
                leaderboard_EventResultsLists = event_result.Select(x => new Leaderboard_EventResultsList
                                                {
                                                    Point = x.Final_Point,
                                                    Runner_Name = x.AppUser.FirstName + " " + x.AppUser.LastName
                                                }).ToList();
            }

            leaderboard_EventResultsLists.Sort();

            ViewData["List"] = leaderboard_EventResultsLists;

            return View();
        }
    }
}
