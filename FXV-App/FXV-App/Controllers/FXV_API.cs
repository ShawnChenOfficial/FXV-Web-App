using System;
using System.Collections.Generic;
using System.Linq;
using FXV.Data;
using FXV.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using NPOI.SS.Formula.Functions;
using FXV.ViewModels.NewModels;
using FXV_App.CustomizeControllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    [Route("api/{action}")]
    [Authorize]
    public class CommonController : CustomizeController
    {
        public CommonController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        //
        //Get all organizations List
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{num}/{last_id}")]
        public async Task<IActionResult> GetOrgList(int num, int last_id)
        {
            var result = new List<ViewModel_Organization>();

            var org_ids = new List<int>();

            var role = _claim.Role;

            switch (role)
            {
                case "Admin":
                    result = await _dbContext.Organization
                                    .Where(w => w.Org_ID > last_id && w.Org_ID != 0).OrderBy(o => o.Org_ID).Take(num)
                                    .Include(inc => inc.AppUser)
                                    .Select(s => new ViewModel_Organization
                                    {
                                        OrgId = s.Org_ID,
                                        Location = s.Location,
                                        Name = s.Name,
                                        NumberOfTeams = s.Num_Of_Teams,
                                        NumberOfMembers = _dbContext.Organization_Relationship.Where(ww => ww.Org_ID == s.Org_ID).Count(),
                                        ImgPath = s.Img_Path,
                                        ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                                    }).ToListAsync();
                    break;
                case "Organization":
                    org_ids = _dbContext.Organization_Relationship.Where(x => x.Id == _claim.Uid && x.Role == "Organization Manager").Select(s => s.Org_ID).ToList();
                    result = await _dbContext.Organization.Where(w => w.Org_ID > last_id && org_ids.Contains(w.Org_ID) && w.Org_ID > last_id && w.Org_ID != 0)
                                    .OrderBy(o => o.Org_ID)
                                    .Take(num)
                                    .Include(inc => inc.AppUser)
                                    .Select(s => new ViewModel_Organization
                                    {
                                        OrgId = s.Org_ID,
                                        Location = s.Location,
                                        Name = s.Name,
                                        NumberOfTeams = s.Num_Of_Teams,
                                        NumberOfMembers = _dbContext.Organization_Relationship.Where(ww => ww.Org_ID == s.Org_ID).Count(),
                                        ImgPath = s.Img_Path,
                                        ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                                    }).ToListAsync();
                    break;
                case "Manager":
                    org_ids = _dbContext.Team_Membership.Where(ww => ww.Id == _claim.Uid && ww.Role == "Team Manager" && ww.Position == "Team Manager").Include(inc => inc.Team).Select(s => s.Team.Org_ID).Distinct().ToList();
                    result = await _dbContext.Organization.Where(w => w.Org_ID > last_id && org_ids.Contains(w.Org_ID) && w.Org_ID > last_id && w.Org_ID != 0)
                                    .OrderBy(o => o.Org_ID)
                                    .Take(num)
                                    .Include(inc => inc.AppUser)
                                    .Select(s => new ViewModel_Organization
                                    {
                                        OrgId = s.Org_ID,
                                        Location = s.Location,
                                        Name = s.Name,
                                        NumberOfTeams = s.Num_Of_Teams,
                                        NumberOfMembers = _dbContext.Organization_Relationship.Where(ww => ww.Org_ID == s.Org_ID).Count(),
                                        ImgPath = s.Img_Path,
                                        ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                                    }).ToListAsync();
                    break;
            }

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //
        //Get organization(s) list based on organization name
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{org_name}")]
        public async Task<IActionResult> GetOrgsByName(string org_name)
        {
            var result = new List<ViewModel_Organization>();

            var org_ids = new List<int>();

            var role = _claim.Role;

            switch (role)
            {
                case "Admin":
                    result = await _dbContext.Organization
                                    .Where(w => w.Name.ToLower().Contains(org_name.ToLower()) && w.Org_ID != 0)
                                    .Include(inc => inc.AppUser)
                                    .Select(s => new ViewModel_Organization
                                    {
                                        OrgId = s.Org_ID,
                                        Location = s.Location,
                                        Name = s.Name,
                                        NumberOfTeams = s.Num_Of_Teams,
                                        NumberOfMembers = _dbContext.Organization_Relationship.Where(ww => ww.Org_ID == s.Org_ID).Count(),
                                        ImgPath = s.Img_Path,
                                        ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                                    }).ToListAsync();
                    break;
                case "Organization":
                    org_ids = _dbContext.Organization_Relationship.Where(x => x.Id == _claim.Uid && x.Role == "Organization Manager").Select(s => s.Org_ID).ToList();
                    result = await _dbContext.Organization
                                    .Where(w => org_ids.Contains(w.Org_ID) && w.Org_ID != 0 && w.Name.ToLower().Contains(org_name.ToLower()))
                                    .Include(inc => inc.AppUser)
                                    .Select(s => new ViewModel_Organization
                                    {
                                        OrgId = s.Org_ID,
                                        Location = s.Location,
                                        Name = s.Name,
                                        NumberOfTeams = s.Num_Of_Teams,
                                        NumberOfMembers = _dbContext.Organization_Relationship.Where(ww => ww.Org_ID == s.Org_ID).Count(),
                                        ImgPath = s.Img_Path,
                                        ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                                    }).ToListAsync();
                    break;
                case "Manager":
                    org_ids = _dbContext.Team_Membership.Where(ww => ww.Id == _claim.Uid && ww.Role == "Team Manager" && ww.Position == "Team Manager").Include(inc => inc.Team).Select(s => s.Team.Org_ID).Distinct().ToList();
                    result = await _dbContext.Organization
                                    .Where(w => org_ids.Contains(w.Org_ID) && w.Org_ID != 0 && w.Name.ToLower().Contains(org_name.ToLower()))
                                    .Include(inc => inc.AppUser)
                                    .Select(s => new ViewModel_Organization
                                    {
                                        OrgId = s.Org_ID,
                                        Location = s.Location,
                                        Name = s.Name,
                                        NumberOfTeams = s.Num_Of_Teams,
                                        NumberOfMembers = _dbContext.Organization_Relationship.Where(ww => ww.Org_ID == s.Org_ID).Count(),
                                        ImgPath = s.Img_Path,
                                        ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                                    }).ToListAsync();
                    break;
            }

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //
        //Get all teams list
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{org_id}")]
        public async Task<IActionResult> GetTeamList(int org_id)
        {
            // ensure if user manually changes the org ID on html to view team information that is not supposed to be available to them.
            if (_claim.Role != "Admin")
            {
                if (!HasAccessToViewOrgInfo(org_id))
                {
                    return RedirectToAction("Forbid_403", "Errors");
                }
            }

            List<ViewModel_Team> TeamsList = new List<ViewModel_Team>();

            var role = _claim.Role;

            switch (role)
            {
                case "Admin":
                    TeamsList = await _dbContext.Team.Where(x => x.Org_ID == org_id)
                                .Include(inc => inc.AppUser)
                                .Include(iinc => iinc.Sport.Sport_Name)
                                .Include(iiinc => iiinc.Organization.Name)
                                .Select(s =>
                               new ViewModel_Team
                               {
                                   TeamId = s.Team_ID,
                                   Name = s.Name,
                                   Description = s.Description,
                                   Sport = s.Sport.Sport_Name,
                                   Organization = s.Organization.Name,
                                   Location = s.Location,
                                   Img_Path = s.Img_Path,
                                   NumberOfMembers = s.Num_Of_Members,
                                   ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                               }).ToListAsync();
                    break;
                case "Organization":
                    TeamsList = await _dbContext.Team.Where(x => x.Org_ID == org_id)
                                .Include(inc => inc.AppUser)
                                .Include(iinc => iinc.Sport.Sport_Name)
                                .Include(iiinc => iiinc.Organization.Name)
                                .Select(s =>
                               new ViewModel_Team
                               {
                                   TeamId = s.Team_ID,
                                   Name = s.Name,
                                   Description = s.Description,
                                   Sport = s.Sport.Sport_Name,
                                   Organization = s.Organization.Name,
                                   Location = s.Location,
                                   Img_Path = s.Img_Path,
                                   NumberOfMembers = s.Num_Of_Members,
                                   ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                               }).ToListAsync();
                    break;
                case "Manager": // only can see the teams that this user is the team manager of the team.
                    TeamsList = await _dbContext.Team_Membership.Include(z => z.Team)
                                .Where(w => w.Team.Org_ID == org_id && w.Id == _claim.Uid && w.Role == "Team Manager" && w.Position == "Team Manager")
                                .Select(s => s.Team)
                                .Include(inc => inc.AppUser)
                                .Include(iinc => iinc.Sport.Sport_Name)
                                .Include(iiinc => iiinc.Organization.Name)
                                .Select(s =>
                               new ViewModel_Team
                               {
                                   TeamId = s.Team_ID,
                                   Name = s.Name,
                                   Description = s.Description,
                                   Sport = s.Sport.Sport_Name,
                                   Organization = s.Organization.Name,
                                   Location = s.Location,
                                   Img_Path = s.Img_Path,
                                   NumberOfMembers = s.Num_Of_Members,
                                   ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                               }).ToListAsync();
                    break;
            }

            return new JsonResult(JsonConvert.SerializeObject(TeamsList));
        }

        //
        //Get team(s) based on team name
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{team_name}/{org_id}")]
        public async Task<IActionResult> GetTeamsByName(string team_name, int org_id)
        {// ensure if user manually changes the org ID on html to view team information that is not supposed to be available to them.
            if (_claim.Role != "Admin")
            {
                if (!HasAccessToViewOrgInfo(org_id))
                {
                    return RedirectToAction("Forbid_403", "Errors");
                }
            }

            List<ViewModel_Team> TeamsList = new List<ViewModel_Team>();

            var role = _claim.Role;

            switch (role)
            {
                case "Admin":
                    TeamsList = await _dbContext.Team.Where(x => x.Org_ID == org_id && x.Name.ToLower().Contains(team_name.ToLower()))
                                .Include(inc => inc.AppUser)
                                .Include(iinc => iinc.Sport.Sport_Name)
                                .Include(iiinc => iiinc.Organization.Name)
                                .Select(s =>
                               new ViewModel_Team
                               {
                                   TeamId = s.Team_ID,
                                   Name = s.Name,
                                   Description = s.Description,
                                   Sport = s.Sport.Sport_Name,
                                   Organization = s.Organization.Name,
                                   Location = s.Location,
                                   Img_Path = s.Img_Path,
                                   NumberOfMembers = s.Num_Of_Members,
                                   ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                               }).ToListAsync();
                    break;
                case "Organization":
                    TeamsList = await _dbContext.Team.Where(x => x.Org_ID == org_id && x.Name.ToLower().Contains(team_name.ToLower()))
                                .Include(inc => inc.AppUser)
                                .Include(iinc => iinc.Sport.Sport_Name)
                                .Include(iiinc => iiinc.Organization.Name)
                                .Select(s =>
                               new ViewModel_Team
                               {
                                   TeamId = s.Team_ID,
                                   Name = s.Name,
                                   Description = s.Description,
                                   Sport = s.Sport.Sport_Name,
                                   Organization = s.Organization.Name,
                                   Location = s.Location,
                                   Img_Path = s.Img_Path,
                                   NumberOfMembers = s.Num_Of_Members,
                                   ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                               }).ToListAsync();
                    break;
                case "Manager": // only can see the teams that this user is the team manager of the team.
                    TeamsList = await _dbContext.Team_Membership.Include(z => z.Team)
                                .Where(w => w.Team.Name.ToLower().Contains(team_name.ToLower())
                                && w.Team.Org_ID == org_id && w.Id == _claim.Uid
                                && w.Role == "Team Manager" && w.Position == "Team Manager")
                                .Select(s => s.Team)
                                .Include(inc => inc.AppUser)
                                .Include(iinc => iinc.Sport.Sport_Name)
                                .Include(iiinc => iiinc.Organization.Name)
                                .Select(s =>
                               new ViewModel_Team
                               {
                                   TeamId = s.Team_ID,
                                   Name = s.Name,
                                   Description = s.Description,
                                   Sport = s.Sport.Sport_Name,
                                   Organization = s.Organization.Name,
                                   Location = s.Location,
                                   Img_Path = s.Img_Path,
                                   NumberOfMembers = s.Num_Of_Members,
                                   ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                               }).ToListAsync();
                    break;
            }

            return new JsonResult(JsonConvert.SerializeObject(TeamsList));
        }

        //
        //Get all test(s) list
        //
        [HttpGet("{last_id}")]
        public async Task<IActionResult> GetTestList(int last_id)
        {
            var testList = new List<ViewModel_Test>();

            testList = await _dbContext.Test.Where(w => w.Test_ID > last_id).OrderBy(o => o.Test_ID)
                .Include(inc => inc.Test_Category.Category).OrderBy(o => o.Name).Select(t => new ViewModel_Test
                {
                    TestId = t.Test_ID,
                    Name = t.Name,
                    Gender = t.Gender,
                    Description = t.Description,
                    Public = t.Public,
                    Reverse = t.Reverse,
                    Unit = t.Unit,
                    LowerResult = t.LowerResult,
                    HigherResult = t.HigherResult,
                    LowerScore = t.LowerScore,
                    HigherScore = t.HigherScore,
                    Category = t.Test_Category.Category,
                    Tested = _dbContext.Test_Result.Where(x => x.Test_ID == t.Test_ID).Count(),
                    Status = Status.HasNoRunningActivity,
                    IsSplittable = t.IsSplittable,
                    UsedAsSplit = t.UsedAsSplit
                }).ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(testList));
        }

        //
        //Get the detailed information of a test, and the top 3 athletes information
        //test information including test category and test unit
        //Athlete information including athlete name, gender, image, gender, score, and result
        //
        [HttpGet("{test_id}")]
        public async Task<IActionResult> GetTestDetail(int test_id)
        {

            var test = await _dbContext.Test.Where(w => w.Test_ID == test_id).Include(inc => inc.Test_Category).FirstOrDefaultAsync();

            var detail = await _dbContext.Test_Result.Where(w => w.Test_ID == test_id)
                .Include(iiinc => iiinc.AppUser)
                .OrderBy(o => o.Point)
                .OrderBy(oo => (test.Reverse) ? (0 - oo.Result) : oo.Result)
                .GroupBy(g => g.Id)
                .Select(s => s.FirstOrDefault(single => single.Point == s.Max(m => m.Point)))
                .Take(3)
                .AsNoTracking()
                .ToListAsync();

            var result = new ViewModel_TestDetail
            {
                TestId = test.Test_ID,
                Sys_Permission = _claim.Role,
                Category = test.Test_Category.Category,
                Unit = test.Unit,
                Top = new Top
                {
                    AthleteName = (detail.Count > 0) ? detail[0].AppUser.FirstName + " " + detail[0].AppUser.LastName : "Unknown",
                    Age = (detail.Count > 0) ? GetAge(detail[0].AppUser.DOB) : 0,
                    Gender = (detail.Count > 0) ? detail[0].AppUser.Gender : "Unknown",
                    ImgPath = (detail.Count > 0) ? detail[0].AppUser.Profile_Img_Path : "/sources/userProfileImg/user-profile-null.png",
                    Score = (detail.Count > 0) ? detail[0].Point : 0,
                    Result = (detail.Count > 0) ? detail[0].Result : 0
                },
                Promiex = new Promiex
                {
                    AthleteName = (detail.Count > 1) ? detail[1].AppUser.FirstName + " " + detail[0].AppUser.LastName : "Unknown",
                    Age = (detail.Count > 1) ? GetAge(detail[1].AppUser.DOB) : 0,
                    Gender = (detail.Count > 1) ? detail[1].AppUser.Gender : "Unknown",
                    ImgPath = (detail.Count > 1) ? detail[1].AppUser.Profile_Img_Path : "/sources/userProfileImg/user-profile-null.png",
                    Score = (detail.Count > 1) ? detail[1].Point : 0,
                    Result = (detail.Count > 1) ? detail[1].Result : 0
                },
                Bronze = new Bronze
                {
                    AthleteName = (detail.Count > 2) ? detail[2].AppUser.FirstName + " " + detail[0].AppUser.LastName : "Unknown",
                    Age = (detail.Count > 2) ? GetAge(detail[2].AppUser.DOB) : 0,
                    Gender = (detail.Count > 2) ? detail[2].AppUser.Gender : "Unknown",
                    ImgPath = (detail.Count > 2) ? detail[2].AppUser.Profile_Img_Path : "/sources/userProfileImg/user-profile-null.png",
                    Score = (detail.Count > 2) ? detail[2].Point : 0,
                    Result = (detail.Count > 2) ? detail[2].Result : 0
                }
            };

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        [HttpGet("{testName}")]
        public async Task<IActionResult> GetTestsByName(string testName)
        {
            var testList = await _dbContext.Test.Where(w => w.Name.ToLower().Contains(testName.ToLower()))
                .OrderBy(o => o.Name)
                .Include(inc => inc.Test_Category.Category)
                .Select(t => new ViewModel_Test
                {
                    TestId = t.Test_ID,
                    Name = t.Name,
                    Gender = t.Gender,
                    Description = t.Description,
                    Public = t.Public,
                    Reverse = t.Reverse,
                    Unit = t.Unit,
                    LowerResult = t.LowerResult,
                    HigherResult = t.HigherResult,
                    LowerScore = t.LowerScore,
                    HigherScore = t.HigherScore,
                    Category = t.Test_Category.Category,
                    Tested = _dbContext.Test_Result.Where(x => x.Test_ID == t.Test_ID).Count(),
                    Status = Status.HasNoRunningActivity,
                    IsSplittable = t.IsSplittable,
                    UsedAsSplit = t.UsedAsSplit
                }).ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(testList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testName">The test name user trying to search</param>
        /// <param name="testid">The test id which user is trying to use as the base test for test activity, this param is used to search if any tests can be used as a split for this test in a test activity</param>
        /// <returns></returns>
        [HttpGet("{testName}/{testid}")]
        public async Task<IActionResult> GetSplitTestsByName(string testName, int testid)
        {
            var categoryid_gender = await _dbContext.Test.Where(w => w.Test_ID == testid).Select(s => new
            {
                CategoryId = s.TC_id,
                Gender = s.Gender
            }).FirstOrDefaultAsync();

            var testList = await _dbContext.Test.Where(w => w.TC_id == categoryid_gender.CategoryId
            && w.Gender == categoryid_gender.Gender
            && w.UsedAsSplit == true && ((testName != "") ? w.Name.ToLower().Contains(testName.ToLower()) : true))
                .OrderBy(o => o.Name)
                .Include(inc => inc.Test_Category.Category)
                .Select(t => new ViewModel_Test
                {
                    TestId = t.Test_ID,
                    Name = t.Name
                }).ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(testList));
        }
        //Get all combines list
        [HttpGet("{num}/{last_id}")]
        public async Task<IActionResult> GetCombineList(int num, int last_id)
        {
            var combineList = new List<ViewModel_Combine>();

            if (num == 0)
            {
                combineList = await _dbContext.Combine
                    .Select(s => new ViewModel_Combine
                    {
                        CombineId = s.C_ID,
                        Description = s.Description,
                        Gender = s.Gender,
                        Name = s.Name,
                        Img_Path = s.Img_Path,
                        Tested = _dbContext.Combine_Result.Where(w => w.C_ID == s.C_ID).Count()
                    }).ToListAsync();
            }
            else
            {
                combineList = await _dbContext.Combine.Where(w => w.C_ID > last_id).OrderBy(o => o.C_ID).Take(num)
                    .Select(s => new ViewModel_Combine
                    {
                        CombineId = s.C_ID,
                        Description = s.Description,
                        Gender = s.Gender,
                        Name = s.Name,
                        Img_Path = s.Img_Path,
                        Tested = _dbContext.Combine_Result.Where(w => w.C_ID == s.C_ID).Count()
                    }).ToListAsync();
            }

            return new JsonResult(JsonConvert.SerializeObject(combineList));
        }

        //
        //Get the detailed information of a combine, and the top 3 athletes information
        //combine information including tests belonging to this combine
        //Athlete information including athlete name, gender, image, gender, score, and result
        //
        [HttpGet("{combine_id}")]
        public async Task<IActionResult> GetCombineDetail(int combine_id)
        {

            var combine = await _dbContext.Combine.Where(w => w.C_ID == combine_id).FirstOrDefaultAsync();

            var rank = await _dbContext.Combine_Result.Where(w => w.C_ID == combine_id)
                .Include(iiinc => iiinc.AppUser)
                .OrderBy(o => o.Point)
                .GroupBy(g => g.Id)
                .Select(s => s.FirstOrDefault(single => single.Point == s.Max(m => m.Point)))
                .Take(3)
                .AsNoTracking()
                .ToListAsync();

            var result = new ViewModel_CombineDetail
            {
                Sys_Permission = _claim.Role,
                CombineId = combine.C_ID,
                TestNames = await _dbContext.Combine_Builder.Where(w => w.C_ID == combine_id).Include(inc => inc.Test).OrderBy(o => o.Test.Name).Select(s => s.Test.Name).ToListAsync(),
                Top = new Top
                {
                    AthleteName = (rank.Count > 0) ? rank[0].AppUser.FirstName + " " + rank[0].AppUser.LastName : "Unknown",
                    Age = (rank.Count > 0) ? GetAge(rank[0].AppUser.DOB) : 0,
                    Gender = (rank.Count > 0) ? rank[0].AppUser.Gender : "Unknown",
                    ImgPath = (rank.Count > 0) ? rank[0].AppUser.Profile_Img_Path : "/sources/userProfileImg/user-profile-null.png",
                    Score = (rank.Count > 0) ? rank[0].Point : 0,
                },
                Promiex = new Promiex
                {
                    AthleteName = (rank.Count > 1) ? rank[1].AppUser.FirstName + " " + rank[0].AppUser.LastName : "Unknown",
                    Age = (rank.Count > 1) ? GetAge(rank[1].AppUser.DOB) : 0,
                    Gender = (rank.Count > 1) ? rank[1].AppUser.Gender : "Unknown",
                    ImgPath = (rank.Count > 1) ? rank[1].AppUser.Profile_Img_Path : "/sources/userProfileImg/user-profile-null.png",
                    Score = (rank.Count > 1) ? rank[1].Point : 0
                },
                Bronze = new Bronze
                {
                    AthleteName = (rank.Count > 2) ? rank[2].AppUser.FirstName + " " + rank[0].AppUser.LastName : "Unknown",
                    Age = (rank.Count > 2) ? GetAge(rank[2].AppUser.DOB) : 0,
                    Gender = (rank.Count > 2) ? rank[2].AppUser.Gender : "Unknown",
                    ImgPath = (rank.Count > 2) ? rank[2].AppUser.Profile_Img_Path : "/sources/userProfileImg/user-profile-null.png",
                    Score = (rank.Count > 2) ? rank[2].Point : 0
                }
            };

            return new JsonResult(JsonConvert.SerializeObject(result));
        }
        //
        //Get combine(s) list based on combine name
        //
        [HttpGet("{combine_name}")]
        public async Task<IActionResult> GetCombinesByName(string combine_name)
        {
            var result = await _dbContext.Combine
                        .Where(w => w.Name.ToLower().Contains(combine_name.ToLower()))
                        .OrderBy(o => o.Name)
                        .Select(s => new ViewModel_Combine
                        {
                            CombineId = s.C_ID,
                            Name = s.Name,
                            Description = s.Description,
                            Gender = s.Gender,
                            Img_Path = s.Img_Path
                        }).ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //Get all events list
        [HttpGet("{num}/{last_id}")]
        public async Task<IActionResult> GetEventList(int num, int last_id)
        {
            var eventList = new List<ViewModel_Event>();
            if (num == 0)
            {
                eventList = await _dbContext.Event.Include(inc => inc.Event_Status)
                        .Select(s => new ViewModel_Event
                        {
                            EventId = s.E_ID,
                            Name = s.Name,
                            Description = s.Description,
                            Location = s.Location,
                            Time = s.Time.ToString("h:mm tt"),
                            Date = s.Date.ToString("MMM dd yyyy"),
                            Img_Path = s.Img_Path,
                            Registered = _dbContext.Event_Assigned_Attendee.Where(a => a.E_ID == s.E_ID).Count(),
                            Status = s.Event_Status.Status
                        })
                        .ToListAsync();
            }
            else
            {
                eventList = await _dbContext.Event.Where(w => w.E_ID > last_id).OrderBy(o => o.E_ID).Take(num).Include(inc => inc.Event_Status)
                        .Select(s => new ViewModel_Event
                        {
                            EventId = s.E_ID,
                            Name = s.Name,
                            Description = s.Description,
                            Location = s.Location,
                            Time = s.Time.ToString("h:mm tt"),
                            Date = s.Date.ToString("MMM dd yyyy"),
                            Img_Path = s.Img_Path,
                            Registered = _dbContext.Event_Assigned_Attendee.Where(a => a.E_ID == s.E_ID).Count(),
                            Status = s.Event_Status.Status
                        })
                        .ToListAsync();
            }
            return new JsonResult(JsonConvert.SerializeObject(eventList));
        }

        [HttpGet("{e_id}")]
        public async Task<IActionResult> GetEventCombineDetail(int e_id)
        {
            var event_combine = await _dbContext.Event_Builder.Where(w => w.E_ID == e_id).Include(inc => inc.Combine).FirstOrDefaultAsync();

            var testnames = await _dbContext.Combine_Builder.Where(w => w.C_ID == event_combine.C_ID).Include(inc => inc.Test).Select(s => s.Test.Name).ToListAsync();

            var eventCombineDetail = new ViewModel_CombineDetail
            {
                Combine_Name = event_combine.Combine.Name,
                TestNames = testnames
            };

            return new JsonResult(JsonConvert.SerializeObject(eventCombineDetail));
        }

        //Get event(s) list based on name
        [HttpGet("{event_name}")]
        public async Task<IActionResult> GetEventsByName(string event_name)
        {
            var result = await _dbContext.Event
                        .Where(w => w.Name.ToLower().Contains(event_name.ToLower())).Include(inc => inc.Event_Status).OrderBy(o => o.E_ID)
                        .Select(s => new ViewModel_Event
                        {
                            EventId = s.E_ID,
                            Name = s.Name,
                            Description = s.Description,
                            Location = s.Location,
                            Time = s.Time.ToString("h:mm tt"),
                            Date = s.Date.ToString("MMM dd yyyy"),
                            Img_Path = s.Img_Path,
                            Registered = _dbContext.Event_Assigned_Attendee.Where(a => a.E_ID == s.E_ID).Count(),
                            Status = s.Event_Status.Status
                        })
                        .ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(result));
        }
        //
        //Get event(s) list based on date and time
        //
        [HttpGet("{start_event_date}/{end_event_date}/")]
        public async Task<IActionResult> GetEventsByDate(DateTime start_event_date, DateTime end_event_date)
        {
            var result = await _dbContext.Event
                        .Where(w =>
                           DateTime.Compare(w.Date.Date, start_event_date.Date) >= 0
                           &&
                           DateTime.Compare(w.Date.Date, end_event_date.Date) <= 0
                        )
                        .Select(s => new ViewModel_Event
                        {
                            EventId = s.E_ID,
                            Name = s.Name,
                            Description = s.Description,
                            Location = s.Location,
                            Time = s.Time.ToString("h:mm tt"),
                            Date = s.Date.ToString("MMM dd yyyy"),
                            Img_Path = s.Img_Path,
                            Registered = _dbContext.Event_Assigned_Attendee.Where(a => a.E_ID == s.E_ID).Count(),
                            Status = s.Event_Status.Status
                        })
                        .ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        private async Task<string> GetRoleName(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }

        //Get all users list
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            var result = await _dbContext.Users
                        .Include(inc => inc.SubscriptionPermission)
                        .Select(s => new ViewModel_User
                        {
                            UId = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Gender = s.Gender,
                            Address = s.Address,
                            City = s.City,
                            PhoneNumber = s.PhoneNumber,
                            Email = s.Email,
                            Discription = s.Description,
                            DOB = s.DOB,
                            Profile_Img_Path = s.Profile_Img_Path,
                            RegisterDate = s.RegisterDate,
                            Subscription = s.SubscriptionPermission.Permission
                        })
                        .ToListAsync();

            await SetSystemRoleOnUserAsync(result);

            return new JsonResult(JsonConvert.SerializeObject(result));
        }
        //
        //Get all users list by gender
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{gender}")]
        public async Task<IActionResult> GetUsersByGender(string gender)
        {
            var result = await _dbContext.Users
                        .Where(w => w.Gender == gender)
                        .Include(inc => inc.SubscriptionPermission)
                        .Select(s => new ViewModel_User
                        {
                            UId = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Gender = s.Gender,
                            Address = s.Address,
                            City = s.City,
                            PhoneNumber = s.PhoneNumber,
                            Email = s.Email,
                            Discription = s.Description,
                            DOB = s.DOB,
                            Profile_Img_Path = s.Profile_Img_Path,
                            RegisterDate = s.RegisterDate,
                            Subscription = s.SubscriptionPermission.Permission
                        })
                        .ToListAsync();

            await SetSystemRoleOnUserAsync(result);

            return new JsonResult(JsonConvert.SerializeObject(result));
        }
        //Get user(s) list based on full name
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{fullname}")]
        public async Task<IActionResult> GetUsersByName(string fullname)
        {
            var result = await _dbContext.Users
                        .Where(w => (w.FirstName + " " + w.LastName).Contains(fullname))
                        .Include(inc => inc.SubscriptionPermission)
                        .Select(s => new ViewModel_User
                        {
                            UId = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Gender = s.Gender,
                            Address = s.Address,
                            City = s.City,
                            PhoneNumber = s.PhoneNumber,
                            Email = s.Email,
                            Discription = s.Description,
                            DOB = s.DOB,
                            Profile_Img_Path = s.Profile_Img_Path,
                            RegisterDate = s.RegisterDate,
                            Subscription = s.SubscriptionPermission.Permission
                        })
                        .ToListAsync();

            await SetSystemRoleOnUserAsync(result);

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //
        //Get user(s) list based on org ids, and used on organization information page
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{org_id}/{num}/{last_id}")]
        public async Task<IActionResult> GetUsersByOrgId(int org_id, int num, int last_id)
        {
            var userIdsInOrg = await _dbContext.Organization_Relationship
                            .Where(w => w.Org_ID == org_id)
                            .Select(s => s.Id).ToListAsync();

            var result = await _dbContext.Team_Membership
                .Where(w => userIdsInOrg.Contains(w.Id) && w.TM_ID > last_id)
                .Include(iinc => iinc.Team)
                .Where(ww => ww.Team.Org_ID == org_id)
                .OrderBy(o => o.TM_ID)
                .Take(num)
                .Include(inc => inc.AppUser)
                .Select(s => new ViewModel_OrgMember
                {
                    List_Id = s.TM_ID,
                    UId = s.AppUser.Id,
                    Age = GetAge(s.AppUser.DOB),
                    Gender = s.AppUser.Gender,
                    Img_Path = s.AppUser.Profile_Img_Path,
                    Name = s.AppUser.FirstName + " " + s.AppUser.LastName,
                    Position = s.Role,
                    Team_Name = s.Team.Name
                }).ToListAsync();

            //if last_id is 0, means this is the first time to retrieve the data, then insert the org manager on the top of the list

            var OrgManager = await _dbContext.Organization.Where(w => w.Org_ID == org_id)
                .Include(inc => inc.AppUser).FirstOrDefaultAsync();

            if (last_id == 0)
            {
                result.Insert(0, new ViewModel_OrgMember
                {
                    List_Id = 0,
                    UId = OrgManager.AppUser.Id,
                    Age = GetAge(OrgManager.AppUser.DOB),
                    Gender = OrgManager.AppUser.Gender,
                    Img_Path = OrgManager.AppUser.Profile_Img_Path,
                    Name = OrgManager.AppUser.FirstName + " " + OrgManager.AppUser.LastName,
                    Position = "Organization Manager",
                    Team_Name = "-"
                });
            }

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //
        //Get user(s) list based on a org id and user name, and used on organization information page
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{org_id}/{name}")]
        public async Task<IActionResult> GetUsersByOrgId_UserName(int org_id, string name)
        {
            var userIdsInOrg = await _dbContext.Organization_Relationship
                            .Where(w => w.Org_ID == org_id)
                            .Select(s => s.Id).ToListAsync();

            var result = await _dbContext.Team_Membership
                .Where(w => userIdsInOrg.Contains(w.Id))
                .Include(iinc => iinc.Team)
                .Where(ww => ww.Team.Org_ID == org_id)
                .Include(inc => inc.AppUser)
                .Where(ww => (ww.AppUser.FirstName + ww.AppUser.LastName).ToLower().Contains(name.Replace(" ", "").ToLower()))
                .Select(s => new ViewModel_OrgMember
                {
                    UId = s.AppUser.Id,
                    Age = GetAge(s.AppUser.DOB),
                    Gender = s.AppUser.Gender,
                    Img_Path = s.AppUser.Profile_Img_Path,
                    Name = s.AppUser.FirstName + " " + s.AppUser.LastName,
                    Position = s.Role,
                    Team_Name = s.Team.Name
                }).ToListAsync();

            // check if the org owner name contains the input, as the org owner may be not in the team membership table
            var OrgManager = await _dbContext.Organization.Where(w => w.Org_ID == org_id)
                .Include(inc => inc.AppUser).Where(ww => (ww.AppUser.FirstName + ww.AppUser.LastName).ToLower().Contains(name.Replace(" ", "").ToLower())).FirstOrDefaultAsync();

            if (OrgManager != null)
            {
                result.Insert(0, new ViewModel_OrgMember
                {
                    UId = OrgManager.AppUser.Id,
                    Age = GetAge(OrgManager.AppUser.DOB),
                    Gender = OrgManager.AppUser.Gender,
                    Img_Path = OrgManager.AppUser.Profile_Img_Path,
                    Name = OrgManager.AppUser.FirstName + " " + OrgManager.AppUser.LastName,
                    Position = "Organization Manager",
                    Team_Name = "-"
                });
            }

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //Get user(s) list based on a team id, and used on team page
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{team_id}/{num}/{last_id}")]
        public async Task<IActionResult> GetUsersByTeamId(int team_id, int num, int last_id)
        {
            var result = await _dbContext.Team_Membership
                .Where(w => w.Team_ID == team_id && w.TM_ID > last_id)
                .Include(inc => inc.AppUser)
                .OrderBy(o => o.TM_ID).Take(num)
                .Select(s => new ViewModel_TeamMember
                {
                    List_Id = s.TM_ID,
                    UId = s.AppUser.Id,
                    Age = GetAge(s.AppUser.DOB),
                    Gender = s.AppUser.Gender,
                    Img_Path = s.AppUser.Profile_Img_Path,
                    Name = s.AppUser.FirstName + " " + s.AppUser.LastName,
                    Position = s.Role,
                    Location = (s.AppUser.City == "" || s.AppUser.City == null) ? "-" : s.AppUser.City
                }).ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //Get user(s) list based on a team id and user name, and used on team page
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{team_id}/{name}")]
        public async Task<IActionResult> GetUsersByTeamId_UserName(int team_id, string name)
        {
            var result = await _dbContext.Team_Membership
                .Where(w => w.Team_ID == team_id)
                .Include(inc => inc.AppUser)
                .Where(ww => (ww.AppUser.FirstName + ww.AppUser.LastName).ToLower().Contains(name.Replace(" ", "").ToLower()))
                .OrderBy(o => o.TM_ID)
                .Select(s => new ViewModel_TeamMember
                {
                    List_Id = s.TM_ID,
                    UId = s.AppUser.Id,
                    Age = GetAge(s.AppUser.DOB),
                    Gender = s.AppUser.Gender,
                    Img_Path = s.AppUser.Profile_Img_Path,
                    Name = s.AppUser.FirstName + " " + s.AppUser.LastName,
                    Position = s.Role,
                    Location = (s.AppUser.City == "" || s.AppUser.City == null) ? "-" : s.AppUser.City
                }).ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //Get user(s) list based on a team id
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet("{permission_id}")]
        public async Task<IActionResult> GetUsersByPermission(int permission_id)
        {
            var result = await _dbContext.Users
                           .Where(w => w.P_ID == permission_id)
                           .Include(inc => inc.SubscriptionPermission)
                           .Select(s => new ViewModel_User
                           {
                               UId = s.Id,
                               FirstName = s.FirstName,
                               LastName = s.LastName,
                               Gender = s.Gender,
                               Address = s.Address,
                               City = s.City,
                               PhoneNumber = s.PhoneNumber,
                               Email = s.Email,
                               Discription = s.Description,
                               DOB = s.DOB,
                               Profile_Img_Path = s.Profile_Img_Path,
                               RegisterDate = s.RegisterDate,
                               Subscription = s.SubscriptionPermission.Permission
                           })
                           .ToListAsync();

            result = await SetSystemRoleOnUserAsync(result);

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //Get get athlete statistic on profile based on a user id
        [HttpGet("{user_id}")]
        public async Task<IActionResult> GetAthleteCurrentStatistic(int user_id)
        {
            var result = await _dbContext.Combine_Result.Where(x => x.Id == user_id)
                         .OrderByDescending(o => o.CR_ID).Select(s =>
                           new ViewModel_AthleteCurrentStatistic
                           {
                               Height = s.Height,
                               Weight = s.Weight,
                               Wingspan = s.Wingspan,
                               Handspan = s.Handspan,
                               StandingReach = s.Standing_Reach,
                               DominantHand = s.Dominant_Hand
                           }).FirstOrDefaultAsync();

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //Get get athlete performance data on profile based on a user id
        [HttpGet("{user_id}")]
        public async Task<IActionResult> GetAthletePerformanceData(int user_id)
        {
            var test_Results = _dbContext.Test_Result.Where(x => x.Id == user_id && x.Point != 0)
                         .Include(inc => inc.Test.Test_Category).OrderBy(o => o.Test.Test_Category).AsNoTracking();

            var result = new List<ViewModel_AthleteOverallPerformanceData>();

            if (test_Results.Count() != 0)
            {
                var categories = await _dbContext.Test_Category.ToListAsync();

                foreach (var x in categories)
                {
                    result.Add(new ViewModel_AthleteOverallPerformanceData
                    {
                        Category = x.Category
                    });
                }

                foreach (var x in result)
                {
                    var scoreInCategory = await test_Results.Where(w => w.Test.Test_Category.Category == x.Category).ToListAsync();

                    if (scoreInCategory.Sum(s => s.Point) != 0)
                    {
                        x.Score = (scoreInCategory.Sum(m => m.Point) / scoreInCategory.Count) / 100 + 20;
                    }
                    else
                    {
                        x.Score = 20;
                    }
                }
            }
            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //Get get athlete combine result data on profile based on a user id
        [HttpGet("{user_id}")]
        public async Task<IActionResult> GetAthleteCombineRecords(int user_id)
        {
            var result = await _dbContext.Combine_Result.Where(w => w.Id == user_id && w.Point != 0).Include(inc => inc.Combine).GroupBy(g => g.C_ID)
                .Select(s => new ViewModel_AthleteCombineRecord
                {
                    C_ID = s.First().C_ID,
                    CombineName = s.First().Combine.Name,
                    TopScore = 0,
                    Rank = 0,
                    BestAttemp = s.Max(m => m.Point)
                }).ToListAsync();

            foreach (var x in result)
            {
                var allRecord = await _dbContext.Combine_Result.Where(w => w.C_ID == x.C_ID)
                            .GroupBy(g => g.Id).Select(s => new
                            {
                                Score = s.Max(m => m.Point),
                                s.First().Id
                            }).ToListAsync();

                x.Rank = allRecord.Where(w => w.Score > x.BestAttemp).Count() + 1;

                x.TopScore = allRecord.FirstOrDefault().Score;

                x.TestNames = await _dbContext.Combine_Builder.Where(w => w.C_ID == x.C_ID).Include(inc => inc.Test).Select(s => s.Test.Name).ToListAsync();
            }
            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //Get get athlete organizations and teams on profile based on a user id
        [HttpGet("{user_id}")]
        public async Task<IActionResult> GetAthleteOrgsTeams(int user_id)
        {

            var result = await _dbContext.Organization_Relationship.Where(w => w.Id == user_id)
                .Include(inc => inc.Organization).Select(s => new ViewModel_AthleteOrgsTeams
                {
                    Org_ID = s.Org_ID,
                    Organization = s.Organization.Name
                }).AsNoTracking().ToListAsync();

            foreach (var x in result)
            {
                x.Teams.AddRange(await _dbContext.Team_Membership.Where(w => w.Id == user_id)
                    .Include(inc => inc.Team).Where(w => w.Team.Org_ID == x.Org_ID)
                    .Select(s => new ViewModel_Team
                    {
                        Img_Path = s.Team.Img_Path,
                        Name = s.Team.Name
                    }).ToListAsync());
            }

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        //
        // Some private Api Tools that are only used in this library
        //

        private async Task<List<ViewModel_User>> SetSystemRoleOnUserAsync(List<ViewModel_User> result)
        {
            foreach (var x in result)
            {
                x.SystemRole = (await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(x.UId.ToString()))).FirstOrDefault();
            }

            return result;
        }


        public int GetAge(DateTime DOB)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - DOB.Year;

            // Go back to the year the person was born in case of a leap year
            if (DOB.Date > today.AddYears(-age)) age--;

            return age;
        }
    }


    [Route("api/[Controller]")]
    public class DataVisualization
    {
        // Dashborad diagramming
        // Profile diagramming
        // Data Report Diagramming
    }


    [Route("api/[Controller]")]
    public class DataReporting
    {

    }

    [Route("api/[Controller]")]
    public class BulkImport
    {

    }
}
