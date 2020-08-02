using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FXV_App.CustomizeControllers;
using FXV.Data;
using FXV.JwtManager;
using FXV.Models;
using FXV.PasswordHasher;
using FXV.TestsFormula;
using FXV.ViewModels;
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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Serilog;
using Serilog.Formatting.Compact;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class AllUsersController : CustomizeController
    {

        const string SOMETHING = "const type what ever it is global or not";
        string _something = "only applied for global params within the customised base controller";
        string Something = "only for global params within the clild controller";
        string something = "only applied within a action/method of a controller";


        public AllUsersController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult AllUsers()
        {

            return View();
        }

        private bool HasAccess(int org_id)
        {
            return _dbContext.Organization_Relationship.Where(w => w.Id == _claim.Uid && w.Role == "Organization Manager" && w.Org_ID == org_id).Count() > 0 || _dbContext.Team_Membership.Where(w => w.Id == _claim.Uid && w.Role == "Team Manager" && w.Position == "Team Manager").Include(inc => inc.Team).Where(ww => ww.Team.Org_ID == org_id).Count() > 0;
        }

        //This method will be shared by both admin user list and organization user list
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetMoreSystemUsers(int last_user_id, int? org_id)
        {
            var role = _claim.Role;

            var num = int.Parse(_configuration["NumbersOfAllUsersList:Value"]);

            if (org_id == 0 || org_id == null)
            {
                // ensure if user manually changes the org ID on html to view team information that is not supposed to be available to them.
                if (_claim.Role != "Admin")
                {
                    return RedirectToAction("Forbid_403", "Errors");
                }
                List<AppUser> usersList = null;


                usersList = _dbContext.Users.Where(w => w.Id > last_user_id).Take(num).ToList();

                return Content(JsonConvert.SerializeObject(usersList), "application/json");
            }
            else
            {
                // ensure if user manually changes the org ID on html to view team information that is not supposed to be available to them.
                if (_claim.Role != "Admin")
                {
                    if (!HasAccess((int)org_id))
                    {
                        return RedirectToAction("Forbid_403", "Errors");
                    }
                }

                OrganizationUsersList usersList = new OrganizationUsersList();

                usersList.AdminAccess = role == "Admin" || role == "Organization" || (role == "Manager" && _dbContext.Team_Membership.Where(w => w.Id == _claim.Uid && w.Role == "Team Manager" && w.Position == "Team Manager").Include(inc => inc.Team).Select(s => s.Team.Org_ID).ToList().Contains((int)org_id)); ;

                if (_claim.Role == "Organization" || _claim.Role == "Admin")
                {
                    List<int> Ids = _dbContext.Organization_Relationship.Where(w => w.Org_ID == org_id).Select(s => s.Id).ToList();

                    usersList.OrganizationUsers = _dbContext.Organization_Relationship.Where(w => w.Org_ID == org_id && w.Id > last_user_id).Take(num)
                                                    .Include(inc => inc.AppUser).Select(s => s.AppUser).ToList();
                }
                else if (_claim.Role == "Manager")
                {
                    var team_ids = _dbContext.Team_Membership.Where(w => w.Id == _claim.Uid && w.Role == "Team Manager" && w.Position == "Team Manager")
                                                    .Include(inc => inc.Team).Where(ww => ww.Team.Org_ID == org_id).Select(s=>s.Team_ID).ToList();
                    usersList.OrganizationUsers = _dbContext.Team_Membership.Where(w=>team_ids.Contains(w.Team_ID)).Select(s => s.AppUser).OrderBy(o=>o.Id).Where(ww => ww.Id > last_user_id).Distinct().Take(num).ToList();
                }
                return Content(JsonConvert.SerializeObject(usersList), "application/json");
            }

        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult CreateSystemUsers()
        {
            return View();
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> CreateSystemUsers(NewSystemUser newSystemUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newSystemUser);
            }
            else
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(newSystemUser.Email);

                    if (user != null)
                    {
                        TempData["Error"] = "This email has registered, please change to another email address.";
                        return View(newSystemUser);
                    }

                    if (user == null)
                    {

                        using (var transaction = _dbContext.Database.BeginTransaction())
                        {
                            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                            byte[] buffer = new byte[16];

                            rng.GetBytes(buffer);

                            string salt = BitConverter.ToString(buffer);

                            string salt1 = salt.Substring(0, salt.Length / 2);
                            string salt2 = salt.Substring(salt.Length / 2);

                            user = new AppUser
                            {
                                UserName = newSystemUser.Email,
                                Email = newSystemUser.Email,
                                FirstName = newSystemUser.FirstName,
                                LastName = newSystemUser.LastName,
                                Address = newSystemUser.Address,
                                City = newSystemUser.City,
                                Gender = newSystemUser.Gender,
                                DOB = newSystemUser.DOB,
                                Salt_1 = salt1,
                                Salt_2 = salt2,
                                PasswordHash = new PasswordHandler().GetEncrptedPWD("00000000", salt1, salt2),
                                SecurityStamp = "",
                                ConcurrencyStamp = "",
                                P_ID = 1,
                                PhoneNumber = newSystemUser.PhoneNumber,
                                EmailConfirmed = true
                            };

                            var result = await _userManager.CreateAsync(user);

                            if (result.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(user, "Athlete");
                            }

                            transaction.Commit();
                        }
                        return RedirectToAction("AllUsers");
                    }
                    return View(newSystemUser);
                }
                catch (Exception e)
                {
                    return View(newSystemUser);
                }
            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string GetAllOrganizations()
        {
            List<Organization> list = new List<Organization>();

            list = _dbContext.Organization.ToList();

            return JsonConvert.SerializeObject(list);
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string GetTeams(int org_id)
        {

            List<Team> list = new List<Team>();

            list = _dbContext.Team.Where(x => x.Org_ID == org_id).ToList();

            return JsonConvert.SerializeObject(list);
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<string> CreateSystemUsersWithCombineThruFile(IFormFile fileWithCombine, bool fileIsXLSX, bool signedToOrgWithCombine, int org_id, bool signedToTeamWithCombine, int team_id)
        {
            var logFileName = DateTime.Now;
            var LoggerFileName = ("SystemLog/FileImportWithCombine/" + logFileName.ToShortDateString().Replace('/', '-') + " - " + logFileName.ToShortTimeString().Replace('/', '-') + ".txt");

            List<string> failedStoredUsers = new List<string>();
            List<string> existingUsers = new List<string>();
            List<string> storedUsers = new List<string>();

            List<string> failedSignInOrg = new List<string>();
            List<string> failedSignInTeam = new List<string>();

            FileReaderWithResultError fileReaderWithResultError = new FileReaderWithResultError();


            List<string> tests_From_File = new List<string>();
            List<int> combines_From_File = new List<int>();
            List<int> events_From_File = new List<int>();
            List<string> orgs_From_File = new List<string>();
            List<string> teams_From_File = new List<string>();

            bool correctFileType = true;

            if (!string.Equals(fileWithCombine.ContentType, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(fileWithCombine.ContentType, "application/vnd.ms-excel", StringComparison.OrdinalIgnoreCase))
            {
                correctFileType = false;
            }

            if (correctFileType)
            {
                ISheet sheet = new XSSFSheet();
                switch (fileIsXLSX)
                {
                    case true:
                        var workbookXSSF = new XSSFWorkbook(fileWithCombine.OpenReadStream());
                        sheet = workbookXSSF.GetSheetAt(0);
                        break;
                    case false:
                        var workbookHSSF = new HSSFWorkbook(fileWithCombine.OpenReadStream());
                        sheet = workbookHSSF.GetSheetAt(0);
                        break;
                }

                string gender_From_File = "";
                int combine_name_at_cell = 0;
                int event_name_at_cell = 0;
                int org_name_at_cell = 0;
                int team_name_at_cell = 0;
                int height_at_cell = 0;
                int weight_at_cell = 0;
                int wingspan_at_cell = 0;
                int standingreach_name_at_cell = 0;
                int handspan_at_cell = 0;
                int dominanthand_at_cell = 0;

                IRow headers = sheet.GetRow(0);

                int _tests_From_File_Start = 0;
                int _tests_From_File_Start_At = 0;
                int _tests_From_File_Start_End = 0;

                for (int i = 0; i < headers.PhysicalNumberOfCells; i++)
                {
                    if (headers.GetCell(i).ToString().Equals("Gender"))
                    {
                        gender_From_File = sheet.GetRow(1).GetCell(i).ToString();
                    }

                    else if (headers.GetCell(i).ToString().Equals("CombineID"))
                    {
                        combine_name_at_cell = i;
                    }

                    else if (headers.GetCell(i).ToString().Equals("EventID"))
                    {
                        event_name_at_cell = i;
                    }

                    else if (headers.GetCell(i).ToString().Equals("Organization"))
                    {
                        org_name_at_cell = i;
                    }

                    else if (headers.GetCell(i).ToString().Equals("Team"))
                    {
                        team_name_at_cell = i;
                    }
                    else if (headers.GetCell(i).ToString().Equals("Height"))
                    {
                        height_at_cell = i;
                    }

                    else if (headers.GetCell(i).ToString().Equals("Weight"))
                    {
                        weight_at_cell = i;
                    }

                    else if (headers.GetCell(i).ToString().Equals("Wingspan"))
                    {
                        wingspan_at_cell = i;
                    }

                    else if (headers.GetCell(i).ToString().Equals("Standing Reach"))
                    {
                        standingreach_name_at_cell = i;
                    }

                    else if (headers.GetCell(i).ToString().Equals("Handspan"))
                    {
                        handspan_at_cell = i;
                    }

                    else if (headers.GetCell(i).ToString().Equals("Dominant Hand"))
                    {
                        dominanthand_at_cell = i;
                    }
                    else
                    {
                        if (headers.GetCell(i).ToString().Equals("#"))
                        {
                            _tests_From_File_Start++;

                            if (_tests_From_File_Start == 1)
                            {
                                _tests_From_File_Start_At = i + 1;
                            }
                            else if (_tests_From_File_Start == 2)
                            {
                                _tests_From_File_Start_End = i;
                            }
                        }

                        else if (_tests_From_File_Start == 2)
                        {
                            break;
                        }
                        else
                        {
                            switch (_tests_From_File_Start)
                            {
                                case 1:
                                    tests_From_File.Add(headers.GetCell(i).ToString() + ' ' + gender_From_File.Substring(0, 1).ToUpper() + gender_From_File.Substring(1));
                                    break;
                            }
                        }
                    }
                }

                List<string> unexistingTests = new List<string>();

                foreach (var x in tests_From_File)
                {
                    var count = _dbContext.Test.Where(w => w.Name == x).ToList().Count;

                    if (count == 0)
                    {
                        unexistingTests.Add(x);
                    }
                }

                if (unexistingTests.Count == 0)
                {
                    for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
                    {
                        IRow cc = sheet.GetRow(i);

                        var combine_name = int.Parse(sheet.GetRow(i).GetCell(combine_name_at_cell).ToString());

                        if (!combines_From_File.Contains(combine_name))
                        {
                            if (combine_name != 0)
                            {
                                combines_From_File.Add(combine_name);
                            }
                        }

                    }

                    List<int> unexistingCombines = new List<int>();

                    foreach (var x in combines_From_File)
                    {
                        var count = _dbContext.Combine.Where(w => w.C_ID == x).ToList().Count;

                        if (count == 0)
                        {
                            unexistingCombines.Add(x);
                        }
                    }

                    if (unexistingCombines.Count == 0)
                    {
                        for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
                        {
                            var id = int.Parse(sheet.GetRow(i).GetCell(event_name_at_cell).ToString());

                            if (!events_From_File.Contains(id))
                            {
                                if (id != 0)
                                {
                                    events_From_File.Add(id);
                                }
                            }

                        }

                        List<int> unexistingEvents = new List<int>();

                        foreach (var x in events_From_File)
                        {
                            var count = _dbContext.Event.Where(w => w.E_ID == x).ToList().Count;

                            if (count == 0)
                            {
                                unexistingEvents.Add(x);
                            }
                        }

                        if (unexistingEvents.Count == 0)
                        {

                            AppUser newUser = new AppUser();

                            for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
                            {
                                var tests_result_left = tests_From_File.Count; // get the count number of tests in each row of the excel file which have not ben proceesed

                                IRow row = sheet.GetRow(i);

                                int count = 0;

                                bool userHasStored = false;

                                for (int ii = 0; ii < row.PhysicalNumberOfCells; ii++)
                                {
                                    count++;

                                    ICell cell = row.GetCell(ii);

                                    if (cell == null)
                                    {
                                        cell = row.CreateCell(ii);
                                        cell.SetCellValue("");
                                    }

                                    switch (sheet.GetRow(0).GetCell(ii).ToString())
                                    {
                                        case "Username":
                                            newUser.UserName = cell.ToString();
                                            newUser.Email = cell.ToString();
                                            newUser.NormalizedEmail = cell.ToString().ToUpper();
                                            newUser.NormalizedUserName = cell.ToString().ToUpper();
                                            break;
                                        case "Name":
                                            newUser.FirstName = cell.ToString().Substring(0, cell.ToString().IndexOf(' '));
                                            newUser.LastName = cell.ToString().Substring(cell.ToString().IndexOf(' '));
                                            break;
                                        case "Birth date":
                                            newUser.DOB = DateTime.Parse(cell.ToString());
                                            break;
                                        case "Gender":
                                            newUser.Gender = cell.ToString().Substring(0, 1).ToUpper() + cell.ToString().Substring(1);
                                            break;
                                        case "Organization":
                                            if (!signedToOrgWithCombine)
                                            {
                                                org_id = _dbContext.Organization.Where(x => x.Name == cell.ToString()).First().Org_ID;
                                            }
                                            break;
                                        case "Team":
                                            if (!signedToTeamWithCombine && cell.ToString() != "")
                                            {
                                                team_id = _dbContext.Team.Where(x => x.Name == cell.ToString()).First().Team_ID;
                                            }
                                            break;
                                    }

                                    bool isExistingUser = _dbContext.Users.Where(x => x.UserName == newUser.UserName).ToList().Count > 0;

                                    if (isExistingUser == false && count == 4 && userHasStored == false)
                                    {
                                        //store user as new system user

                                        try
                                        {
                                            userHasStored = true;

                                            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                                            byte[] buffer = new byte[16];

                                            rng.GetBytes(buffer);

                                            string salt = BitConverter.ToString(buffer);

                                            string salt1 = salt.Substring(0, salt.Length / 2);
                                            string salt2 = salt.Substring(salt.Length / 2);

                                            newUser.Salt_1 = salt1;
                                            newUser.Salt_2 = salt2;
                                            newUser.PasswordHash = new PasswordHandler().GetEncrptedPWD("00000000", salt1, salt2);
                                            newUser.SecurityStamp = "";
                                            newUser.ConcurrencyStamp = "";
                                            newUser.P_ID = 1;
                                            newUser.EmailConfirmed = true;

                                            var dbResult = await _userManager.CreateAsync(newUser);

                                            if (!dbResult.Succeeded)
                                            {
                                                var user = newUser.UserName;
                                                failedStoredUsers.Add(user);
                                            }
                                            else
                                            {
                                                var role_assign_result = await _userManager.AddToRoleAsync(newUser, "Athlete");

                                                if (!role_assign_result.Succeeded)
                                                {
                                                    var user = newUser.UserName;
                                                    failedStoredUsers.Add(user);
                                                }

                                                storedUsers.Add(row.GetCell(0).ToString());
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            //catch insert exception

                                            fileReaderWithResultError.StoreUsersFaild.Add(new ErrorDetail
                                            {
                                                UserAccount = newUser.UserName,
                                                Column = sheet.GetRow(0).GetCell(0).ToString(),
                                                Exception = e.Message
                                            });
                                        }
                                    }
                                    else if (isExistingUser && count == 4 && userHasStored == false)
                                    {
                                        newUser = _dbContext.Users.Where(x => x.UserName == newUser.UserName).First();
                                    }

                                    else if (isExistingUser && count == _tests_From_File_Start_End - 1 && userHasStored == false)
                                    {
                                        var user = newUser.UserName;
                                        existingUsers.Add(user);

                                        //catch insert exception

                                        fileReaderWithResultError.StoreUsersFaild.Add(new ErrorDetail
                                        {
                                            UserAccount = user,
                                            Column = sheet.GetRow(0).GetCell(0).ToString(),
                                            Exception = "Duplicate Record"
                                        });
                                    }

                                    if (count == _tests_From_File_Start_End - 1)
                                    {
                                        if (signedToOrgWithCombine && org_id != 0)
                                        {
                                            var already_in_org = _dbContext.Organization_Relationship.Where(w => w.Org_ID == org_id && w.Id == newUser.Id).ToList().Count > 0;

                                            if (!already_in_org)
                                            {
                                                //assign user in organization if required

                                                try
                                                {
                                                    Organization_Relationship or_re = new Organization_Relationship
                                                    {
                                                        Id = newUser.Id,
                                                        Org_ID = org_id,
                                                        Role = "Athlete"
                                                    };

                                                    _dbContext.Organization_Relationship.Update(or_re);
                                                }
                                                catch (Exception e)
                                                {
                                                    //catch insert exception

                                                    fileReaderWithResultError.AssignUsersToOrgFaild.Add(new ErrorDetail
                                                    {
                                                        UserAccount = newUser.UserName,
                                                        Exception = e.Message
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                //catch insert exception

                                                fileReaderWithResultError.AssignUsersToOrgFaild.Add(new ErrorDetail
                                                {
                                                    UserAccount = newUser.UserName,
                                                    Exception = "Duplicate Record"
                                                });
                                            }

                                            if (_dbContext.SaveChanges() > 0 || already_in_org)
                                            {
                                                if ((signedToTeamWithCombine || team_id != 0) && signedToOrgWithCombine)
                                                {
                                                    var already_in_team = _dbContext.Team_Membership.Where(x => x.Team_ID == team_id && x.Id == newUser.Id).ToList().Count > 0;

                                                    if (!already_in_team)
                                                    {
                                                        //assign user in team if required

                                                        try
                                                        {
                                                            Team_Membership team_Memberships = new Team_Membership
                                                            {
                                                                Team_ID = team_id,
                                                                Role = "Athlete",
                                                                Position = "Athlete",
                                                                Id = newUser.Id
                                                            };

                                                            _dbContext.Team_Membership.Update(team_Memberships);
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            //catch insert exception

                                                            fileReaderWithResultError.AssignUsersToTeamFaild.Add(new ErrorDetail
                                                            {
                                                                UserAccount = newUser.UserName,
                                                                Column = "Team",
                                                                Exception = e.Message
                                                            });
                                                        }

                                                    }

                                                    if (_dbContext.SaveChanges() > 0 || already_in_team)
                                                    {
                                                        //update numbers of team members
                                                        var team = _dbContext.Team.Find(team_id);
                                                        team.Num_Of_Members = _dbContext.Team_Membership.Where(w => w.Team_ID == team_id).ToList().Count();
                                                        _dbContext.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        failedSignInTeam.Add(newUser.UserName);

                                                        //catch insert exception

                                                        fileReaderWithResultError.AssignUsersToTeamFaild.Add(new ErrorDetail
                                                        {
                                                            UserAccount = newUser.UserName,
                                                            Column = "Team",
                                                            Exception = "Duplicate Record"
                                                        });
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                failedSignInOrg.Add(newUser.UserName);

                                                //catch insert exception

                                                fileReaderWithResultError.AssignUsersToOrgFaild.Add(new ErrorDetail
                                                {
                                                    UserAccount = newUser.UserName,
                                                    Exception = "Duplicate Record"
                                                });
                                            }
                                        }
                                        //assign user to the event
                                        var _eve = _dbContext.Event.Where(x => x.E_ID == int.Parse(sheet.GetRow(i).GetCell(event_name_at_cell).ToString())).First();

                                        var already_in_event = _dbContext.Event_Assigned_Attendee.Where(w => w.E_ID == _eve.E_ID && w.Id == newUser.Id).ToList().Count > 0;

                                        if (!already_in_event)
                                        {
                                            //assign user in event as attendee

                                            try
                                            {
                                                Event_Assigned_Attendee event_Assigned_Attendees = new Event_Assigned_Attendee
                                                {
                                                    E_ID = _eve.E_ID,
                                                    Id = newUser.Id
                                                };
                                                _dbContext.Event_Assigned_Attendee.Update(event_Assigned_Attendees);

                                                if (_dbContext.SaveChanges() <= 0)
                                                {
                                                    //catch insert exception

                                                    fileReaderWithResultError.AssignUsersToEventFaild.Add(new ErrorDetail
                                                    {
                                                        UserAccount = newUser.UserName,
                                                        Column = "EventID",
                                                        Exception = "Duplicate Record"
                                                    });
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //catch insert exception

                                                fileReaderWithResultError.AssignUsersToEventFaild.Add(new ErrorDetail
                                                {
                                                    UserAccount = newUser.UserName,
                                                    Column = "EventID",
                                                    Exception = e.Message
                                                });
                                            }
                                        }
                                        else
                                        {
                                            //catch insert exception

                                            fileReaderWithResultError.AssignUsersToEventFaild.Add(new ErrorDetail
                                            {
                                                UserAccount = newUser.UserName,
                                                Column = "EventID",
                                                Exception = "Duplicate Record"
                                            });
                                        }
                                    }

                                    var eve_id = int.Parse(sheet.GetRow(i).GetCell(event_name_at_cell).ToString());

                                    if (ii < _tests_From_File_Start_At)
                                    {
                                        continue;
                                    }
                                    else if (ii == _tests_From_File_Start_End)
                                    {
                                        //start combine result

                                        var combine = _dbContext.Combine.Find(int.Parse(sheet.GetRow(i).GetCell(combine_name_at_cell).ToString()));

                                        var existing_combine_result = _dbContext.Combine_Result.Where(w => w.C_ID == combine.C_ID && w.E_ID == eve_id && w.Id == _dbContext.Users.Where(ww => ww.UserName == newUser.UserName).First().Id);

                                        var combine_cate = _dbContext.Combine_Categories_Weight.Where(w => w.C_ID == combine.C_ID).ToList();

                                        var test_Result = _dbContext.Test_Result.Include(inc => inc.Test).Include(iinc => iinc.AppUser).Where(w => w.E_ID == eve_id && w.AppUser.UserName == newUser.UserName)
                                                           .GroupBy(y => y.Test_ID).Select(b => b.Max(z =>
                                                                    new Test_Result
                                                                    {
                                                                        Test_ID = z.Test_ID,
                                                                        Id = z.Id,
                                                                        Point = z.Point,
                                                                        Result = z.Result
                                                                    })).ToList();

                                        float point = 0.00f;

                                        foreach (var item in test_Result)
                                        {
                                            var TC_id = _dbContext.Test.Find(item.Test_ID).TC_id;

                                            var countx = _dbContext.Combine_Builder.Include(inc => inc.Test).Where(w => w.C_ID == combine.C_ID && w.IsWeighted && w.Test.TC_id == TC_id).Select(s => s.Test).ToList().Count;

                                            float percentage_of_cate = (100f / (float)countx) / 100f;
                                            float percentage_of_combine = (float)combine_cate.Where(x => x.TC_ID == TC_id).First().Weight;

                                            point += ((float)item.Point * percentage_of_cate * percentage_of_combine);
                                        }
                                        Combine_Result combine_Results = new Combine_Result();

                                        if (existing_combine_result.ToList().Count == 0)
                                        {
                                            //store combine result

                                            try
                                            {
                                                combine_Results.E_ID = eve_id;
                                                combine_Results.Id = _dbContext.Users.Where(w => w.UserName == newUser.UserName).First().Id;
                                                combine_Results.Handspan = (sheet.GetRow(i).GetCell(handspan_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(handspan_at_cell).ToString()) : 0;
                                                combine_Results.Height = (sheet.GetRow(i).GetCell(height_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(height_at_cell).ToString()) : 0;
                                                combine_Results.Standing_Reach = (sheet.GetRow(i).GetCell(standingreach_name_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(standingreach_name_at_cell).ToString()) : 0;
                                                combine_Results.Weight = (sheet.GetRow(i).GetCell(weight_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(weight_at_cell).ToString()) : 0;
                                                combine_Results.Wingspan = (sheet.GetRow(i).GetCell(wingspan_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(wingspan_at_cell).ToString()) : 0;
                                                combine_Results.Dominant_Hand = sheet.GetRow(i).GetCell(dominanthand_at_cell).ToString();
                                                combine_Results.Point = (int)Math.Round(point);
                                                combine_Results.C_ID = int.Parse(row.GetCell(combine_name_at_cell).ToString());

                                                _dbContext.Combine_Result.Update(combine_Results);

                                                if (_dbContext.SaveChanges() <= 0)
                                                {
                                                    //catch insert exception

                                                    fileReaderWithResultError.StoreCombineResultsFaild.Add(new ErrorDetail
                                                    {
                                                        UserAccount = newUser.UserName,
                                                        Exception = "Duplicate Record"
                                                    });
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //catch insert exception

                                                fileReaderWithResultError.StoreCombineResultsFaild.Add(new ErrorDetail
                                                {
                                                    UserAccount = newUser.UserName,
                                                    Exception = e.Message
                                                });
                                            }
                                        }
                                        else
                                        {
                                            //update combine result if exists

                                            try
                                            {
                                                combine_Results = existing_combine_result.First();

                                                combine_Results.Handspan = (sheet.GetRow(i).GetCell(handspan_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(handspan_at_cell).ToString()) : 0;
                                                combine_Results.Height = (sheet.GetRow(i).GetCell(height_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(height_at_cell).ToString()) : 0;
                                                combine_Results.Standing_Reach = (sheet.GetRow(i).GetCell(standingreach_name_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(standingreach_name_at_cell).ToString()) : 0;
                                                combine_Results.Weight = (sheet.GetRow(i).GetCell(weight_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(weight_at_cell).ToString()) : 0;
                                                combine_Results.Wingspan = (sheet.GetRow(i).GetCell(wingspan_at_cell).ToString() != "") ? double.Parse(sheet.GetRow(i).GetCell(wingspan_at_cell).ToString()) : 0;
                                                combine_Results.Dominant_Hand = sheet.GetRow(i).GetCell(dominanthand_at_cell).ToString();
                                                combine_Results.Point = (int)Math.Round(point);


                                                if (_dbContext.SaveChanges() <= 0)
                                                {
                                                    //catch insert exception

                                                    fileReaderWithResultError.StoreCombineResultsFaild.Add(new ErrorDetail
                                                    {
                                                        UserAccount = newUser.UserName,
                                                        Exception = "Duplicate Record"
                                                    });
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //catch insert exception

                                                fileReaderWithResultError.StoreCombineResultsFaild.Add(new ErrorDetail
                                                {
                                                    UserAccount = newUser.UserName,
                                                    Exception = e.Message
                                                });
                                            }
                                        }

                                        int Id = _dbContext.Users.Where(x => x.UserName == newUser.UserName).First().Id;

                                        var event_Results = _dbContext.Event_Result.Where(w => w.E_ID == eve_id && w.Id == Id);

                                        if (event_Results.ToList().Count == 0)
                                        {
                                            //store event result based on combine result

                                            try
                                            {
                                                Event_Result event_Result = new Event_Result
                                                {
                                                    E_ID = eve_id,
                                                    Final_Point = combine_Results.Point,
                                                    Id = Id
                                                };

                                                _dbContext.Event_Result.Update(event_Result);

                                                if (_dbContext.SaveChanges() <= 0)
                                                {
                                                    //catch insert exception

                                                    fileReaderWithResultError.StoreEventResultsFaild.Add(new ErrorDetail
                                                    {
                                                        UserAccount = newUser.UserName,
                                                        Exception = "Duplicate Record"
                                                    });
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //catch insert exception

                                                fileReaderWithResultError.StoreEventResultsFaild.Add(new ErrorDetail
                                                {
                                                    UserAccount = newUser.UserName,
                                                    Exception = e.Message
                                                });
                                            }
                                        }
                                        else if (event_Results.First().Final_Point < combine_Results.Point)
                                        {
                                            try
                                            {
                                                event_Results.First().Final_Point = combine_Results.Point;

                                                _dbContext.Event_Result.Update(event_Results.First());

                                                if (_dbContext.SaveChanges() <= 0)
                                                {
                                                    //catch insert exception

                                                    fileReaderWithResultError.StoreEventResultsFaild.Add(new ErrorDetail
                                                    {
                                                        UserAccount = newUser.UserName,
                                                        Exception = "Duplicate Record"
                                                    });
                                                }

                                            }
                                            catch (Exception e)
                                            {
                                                //catch insert exception

                                                fileReaderWithResultError.StoreEventResultsFaild.Add(new ErrorDetail
                                                {
                                                    UserAccount = newUser.UserName,
                                                    Exception = e.Message
                                                });
                                            }
                                        }

                                        count = 0; // column counter to be 0
                                        newUser = new AppUser(); // initialize newUser
                                        break;
                                    }

                                    else
                                    {
                                        // store test results

                                        IRow first_row = sheet.GetRow(0);
                                        var test_name = first_row.GetCell(ii).ToString() + ' ' + newUser.Gender;

                                        Test test = new Test();

                                        test = _dbContext.Test.Where(x => x.Name == test_name).First();

                                        if (cell.ToString() != "" && cell.ToString() != null && cell.ToString() != "0")
                                        {
                                            double cellValue = 0.00;

                                            try
                                            {
                                                try
                                                {
                                                    cellValue = double.Parse(cell.ToString(), CultureInfo.CurrentCulture);
                                                }
                                                catch (Exception e)
                                                {
                                                    cellValue = cell.NumericCellValue;
                                                }

                                                int point = (test.LowerResult + test.HigherResult + test.LowerScore + test.HigherScore == 0) ? 0
                                                            : (test.Reverse ? new TestFormula(test.HigherResult, test.LowerResult, test.LowerScore, test.HigherScore, cellValue).GetFinalScore()
                                                            : new TestFormula(test.LowerResult, test.HigherResult, test.LowerScore, test.HigherScore, cellValue).GetFinalScore());

                                                Test_Result test_Result = new Test_Result();

                                                test_Result.Test_ID = test.Test_ID;
                                                test_Result.Result = cellValue;
                                                test_Result.Id = _dbContext.Users.Where(x => x.UserName == newUser.UserName).First().Id;
                                                test_Result.E_ID = eve_id;
                                                test_Result.Date = _dbContext.Event.Find(eve_id).Date;
                                                test_Result.Point = (point < 0) ? 0 : point;

                                                _dbContext.Test_Result.Update(test_Result);

                                                if (_dbContext.SaveChanges() <= 0)
                                                {
                                                    //catch insert exception (Less likely to be happened)

                                                    fileReaderWithResultError.StoreTestResultsFaild.Add(new ErrorDetail
                                                    {
                                                        UserAccount = newUser.UserName,
                                                        Column = sheet.GetRow(0).GetCell(ii).ToString(),
                                                        Exception = "Duplicate Record"
                                                    });
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //catch insert exception

                                                fileReaderWithResultError.StoreTestResultsFaild.Add(new ErrorDetail
                                                {
                                                    UserAccount = newUser.UserName,
                                                    Column = sheet.GetRow(0).GetCell(ii).ToString(),
                                                    Exception = e.Message
                                                });
                                            }
                                        }
                                    }
                                }
                            }


                            ReadingUserListWithCombineFileResult readingUserListWithCombineFileResult = new ReadingUserListWithCombineFileResult
                            {
                                OverAllUsers = _dbContext.Users.ToList().Count,
                                Success = storedUsers.Count,
                                FailedStoredUsers = failedStoredUsers,
                                ExistingUsers = existingUsers,
                                FailedSignInOrg = failedSignInOrg,
                                FailedSignInTeam = failedSignInTeam
                            };

                            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).WriteTo.File(new CompactJsonFormatter(), LoggerFileName).CreateLogger();

                            Log.Information("UserID: " + _claim.Uid + ".IP: " + _claim.IP + ",Action: Data Import Users With Combine Results," + "{@fileReaderWithResultError}:", fileReaderWithResultError);

                            return JsonConvert.SerializeObject(readingUserListWithCombineFileResult);
                        }
                        else
                        {
                            string error = "Event(s) ";

                            foreach (var x in unexistingEvents)
                            {
                                error += x + ", ";
                            }
                            if (unexistingEvents.Count > 1)
                            {
                                error = error.Substring(0, error.Length - 1) + " are not existing, please create event(s).";
                            }
                            else
                            {
                                error = error.Substring(0, error.Length - 1) + " is not existing, please create the event.";
                            }

                            return error;
                        }
                    }
                    else
                    {
                        string error = "Combine(s) ";

                        foreach (var x in unexistingCombines)
                        {
                            error += x + ", ";
                        }
                        if (unexistingCombines.Count > 1)
                        {
                            error = error.Substring(0, error.Length - 1) + " are not existing, please create combine(s).";
                        }
                        else
                        {
                            error = error.Substring(0, error.Length - 1) + " is not existing, please create the combine.";
                        }

                        return error;
                    }

                }
                else
                {
                    string error = "Test(s) ";

                    foreach (var x in unexistingTests)
                    {
                        error += x + ", ";
                    }
                    if (unexistingTests.Count > 1)
                    {
                        error = error.Substring(0, error.Length - 1) + " are not existing, please create test(s).";
                    }
                    else
                    {
                        error = error.Substring(0, error.Length - 1) + " is not existing, please create the test.";
                    }

                    return error;
                }
            }
            else
            {
                return "Invalid file, only accpet the file with the extension of xlsx or xls...";
            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<string> CreateSystemUsersThruFile(IFormFile file, bool fileIsXLSX, bool signedToOrg, int org_id, bool signedToTeam, int team_id)
        {
            var logFileName = DateTime.Now;
            var LoggerFileName = ("SystemLog/FileImportOnlyUser/" + logFileName.ToShortDateString().Replace('/', '-') + " - " + logFileName.ToShortTimeString().Replace('/', '-') + ".txt");

            List<string> failedStoredUsers = new List<string>();
            List<string> existingUsers = new List<string>();
            List<string> storedUsers = new List<string>();

            bool correctFileType = true;

            if (!string.Equals(file.ContentType, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(file.ContentType, "application/vnd.ms-excel", StringComparison.OrdinalIgnoreCase))
            {
                correctFileType = false;
            }

            if (correctFileType)
            {
                ISheet sheet = new XSSFSheet();
                switch (fileIsXLSX)
                {
                    case true:
                        var workbookXSSF = new XSSFWorkbook(file.OpenReadStream());
                        sheet = workbookXSSF.GetSheet("New System Users");
                        break;
                    case false:
                        var workbookHSSF = new HSSFWorkbook(file.OpenReadStream());
                        sheet = workbookHSSF.GetSheet("New System Users");
                        break;
                }

                for (int rows = 1; rows < sheet.PhysicalNumberOfRows; rows++)
                {
                    IRow row = sheet.GetRow(rows);

                    var accountExist = await _userManager.FindByNameAsync(row.GetCell(0).ToString());

                    if (accountExist != null)
                    {
                        existingUsers.Add(row.GetCell(0).ToString());
                    }
                    else
                    {
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                        byte[] buffer = new byte[16];

                        rng.GetBytes(buffer);

                        string salt = BitConverter.ToString(buffer);

                        string salt1 = salt.Substring(0, salt.Length / 2);
                        string salt2 = salt.Substring(salt.Length / 2);

                        AppUser newUser = new AppUser
                        {
                            Salt_1 = salt1,
                            Salt_2 = salt2,
                            PasswordHash = new PasswordHandler().GetEncrptedPWD("00000000", salt1, salt2),
                            SecurityStamp = "",
                            ConcurrencyStamp = "",
                            P_ID = 1,
                            EmailConfirmed = true
                        };

                        for (int cols = 0; cols < row.PhysicalNumberOfCells; cols++)
                        {
                            ICell col = row.GetCell(cols);

                            if (col == null)
                            {
                                col = row.CreateCell(cols);
                                col.SetCellValue("");
                            }

                            var header = sheet.GetRow(0).GetCell(cols).ToString();

                            switch (header)
                            {
                                case "Email":
                                    newUser.Email = col.ToString();
                                    newUser.UserName = col.ToString();
                                    break;
                                case "Firstname":
                                    newUser.FirstName = col.ToString();
                                    break;
                                case "LastName":
                                    newUser.LastName = col.ToString();
                                    break;
                                case "Gender":
                                    newUser.Gender = col.ToString();
                                    break;
                                case "DOB":
                                    newUser.DOB = DateTime.Parse(col.ToString());
                                    break;
                                case "City":
                                    newUser.City = col.ToString();
                                    break;
                                case "Address":
                                    newUser.Address = col.ToString();
                                    break;
                                case "PhoneNumber":
                                    newUser.PhoneNumber = col.ToString();
                                    break;
                            }
                        }

                        var dbResult = await _userManager.CreateAsync(newUser);

                        if (dbResult.Succeeded)
                        {
                            var role_assign_result = await _userManager.AddToRoleAsync(newUser, "Athlete");

                            storedUsers.Add(row.GetCell(0).ToString());

                            if (role_assign_result.Succeeded && signedToOrg)
                            {
                                Organization_Relationship or_re = new Organization_Relationship
                                {
                                    Id = newUser.Id,
                                    Org_ID = org_id,
                                    Role = "Athlete"
                                };

                                _dbContext.Organization_Relationship.Update(or_re);
                                _dbContext.SaveChanges();

                                if (signedToTeam)
                                {
                                    Team_Membership team_Memberships = new Team_Membership
                                    {
                                        Team_ID = team_id,
                                        Role = "Athlete",
                                        Position = "Athlete",
                                        Id = newUser.Id
                                    };

                                    _dbContext.Team_Membership.Update(team_Memberships);
                                    _dbContext.SaveChanges();

                                    var team = _dbContext.Team.Find(team_id);
                                    team.Num_Of_Members = _dbContext.Team_Membership.Where(x => x.Team_ID == team_id).ToList().Count();
                                    _dbContext.SaveChanges();
                                }
                            }
                            else if (!role_assign_result.Succeeded)
                            {
                                failedStoredUsers.Add(row.GetCell(0).ToString());
                            }
                        }
                        else
                        {
                            failedStoredUsers.Add(row.GetCell(0).ToString());
                        }
                    }
                }

                ReadingUserListFileResult readingUserListFileResult = new ReadingUserListFileResult
                {
                    OverAllUsers = _dbContext.Users.ToList().Count,
                    Success = storedUsers.Count,
                    FailedStoredUsers = failedStoredUsers,
                    ExistingUsers = existingUsers
                };

                Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).WriteTo.File(new CompactJsonFormatter(), LoggerFileName).CreateLogger();

                Log.Information("UserID: " + _claim.Uid + ".IP: " + _claim.IP + ",Action: Data Import Only Users," + "{@fileReaderWithResultError}:", readingUserListFileResult);

                return JsonConvert.SerializeObject(readingUserListFileResult);
            }
            else
            {
                return "Invalid file, only accpet the file with the extension of xlsx or xls...";
            }
        }
    }
}
