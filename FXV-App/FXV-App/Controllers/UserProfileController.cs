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
using FXV.ViewModels.NewModels;
using FXV_App.CustomizeControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class UserProfileController : CustomizeController
    {
        public UserProfileController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }
        // GET: /<controller>/

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> Index(int user_id, int? org_id, int? team_id) // org_id and team_id is for the request sent from organization list or team member list
        {
            if (_claim.Role != "Admin")
            {
                if (org_id != 0)
                {
                    if (_dbContext.Organization_Relationship.Where(w => w.Id == _claim.Uid && w.Role == "Organization Manager").Count() == 0)
                    {
                        return RedirectToAction("Forbid_403", "Errors");
                    }
                }
                else if (team_id != 0)
                {
                    if (_dbContext.Team_Membership.Where(w => w.Id == _claim.Uid && w.Role == "Team Manager").Count() == 0)
                    {
                        return RedirectToAction("Forbid_403", "Errors");
                    }
                }
            }

            var user = await _dbContext.Users.Where(w => w.Id == user_id).Include(inc => inc.Nationality).FirstOrDefaultAsync();
            var achievement = await _dbContext.AthleteAchievement.Where(w => w.Id == user.Id).Select(s => s.Achievement).FirstOrDefaultAsync();

            ViewModel_UserProfile userProfile = new ViewModel_UserProfile
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Age = GetAgeByDOB(user.DOB),
                Email = user.Email,
                Nationality = user.Nationality.Name,
                City = (user.City == "" || user.City == null) ? "-" : user.City,
                Description = user.Description,
                TopAchevenment = achievement == "" || achievement == null ? "-" : achievement
            };

            if (user_id == _claim.Uid)
            {
                userProfile.Editable = true;
            }

            return View(userProfile);
        }

        private int GetAgeByDOB(DateTime DOB)
        {
            DateTime now = DateTime.Now;

            int age = now.Year - DOB.Year;

            if (now.Month < DOB.Month || (now.Month == DOB.Month && now.Day < DOB.Day))
            {
                age--;
            }

            return age < 0 ? 0 : age;
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> Edit(int user_id)
        {
            if (user_id != _claim.Uid)
            {
                return RedirectToAction("Forbid_403", "Errors");
            }
            else
            {
                var user = await _dbContext.Users.Where(w => w.Id == user_id).FirstOrDefaultAsync();
                var achievement = _dbContext.AthleteAchievement.Where(w => w.Id == user.Id).Select(s => s.Achievement).ToList();

                UserProfileEdit userProfileEdit = new UserProfileEdit()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    City = user.City,
                    DOB = user.DOB,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    Profile_Img_Path = user.Profile_Img_Path,
                    Nationality_ID = user.Nationality_ID,
                    RowVersion = user.RowVersion,
                    AthleteAchievements = achievement
                };


                ViewBag.Nationalities = new SelectList(_dbContext.Nationality.Where(w => w.Nationality_ID > 1).OrderBy(o => o.Name), "Nationality_ID", "Name", user.Nationality_ID);

                return View(userProfileEdit);
            }
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserProfileEdit userProfileEdit)
        {

            if (userProfileEdit.Id != _claim.Uid)
            {
                return RedirectToAction("Forbid_403", "Errors");
            }
            else
            {
                var ReturnResult = false;

                var originalProfile = await _dbContext.Users.Where(x => x.Id == userProfileEdit.Id).FirstOrDefaultAsync();

                if (!ModelState.IsValid)
                {
                    ReturnResult = false;
                }
                else
                {
                    if (originalProfile == null)
                    {
                        ModelState.AddModelError("", "This user has been removed from the system");
                    }
                    else if (originalProfile != null)
                    {
                        try
                        {
                            var New_Img_Path = originalProfile.Profile_Img_Path;

                            if (userProfileEdit.Profile_Image != null)
                            {
                                var date = Request;
                                var files = Request.Form.Files;
                                long size = files.Sum(f => f.Length);
                                string contentRootPath = _hostingEnvironment.ContentRootPath;
                                IFormFile img = userProfileEdit.Profile_Image;


                                if (img.Length > 0)
                                {
                                    string fileExt = img.FileName;

                                    while (fileExt.Contains('.'))
                                    {
                                        fileExt = fileExt.Substring(fileExt.IndexOf('.') + 1);
                                    }

                                    long fileSize = img.Length;
                                    var newFileName = System.Guid.NewGuid().ToString() + "." + fileExt;
                                    string webRootPath = _hostingEnvironment.WebRootPath;
                                    var filePath = webRootPath + "/sources/userProfileImg/" + newFileName;
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        await img.CopyToAsync(stream);
                                    }
                                    New_Img_Path = "/sources/userProfileImg/" + newFileName;
                                }
                            }

                            var entry = _dbContext.Entry(originalProfile);

                            entry.Property("RowVersion").OriginalValue = userProfileEdit.RowVersion;

                            if (await TryUpdateModelAsync<AppUser>(originalProfile, "", x => x.FirstName, x => x.LastName, x => x.PhoneNumber, x => x.Address, x => x.City, x => x.DOB, x => x.Profile_Img_Path, x => x.Nationality_ID))
                            {
                                try
                                {

                                    using (var transaction = _dbContext.Database.BeginTransaction())
                                    {
                                        originalProfile.FirstName = userProfileEdit.FirstName;
                                        originalProfile.LastName = userProfileEdit.LastName;
                                        originalProfile.Nationality_ID = userProfileEdit.Nationality_ID;
                                        originalProfile.PhoneNumber = userProfileEdit.PhoneNumber;
                                        originalProfile.Address = userProfileEdit.Address;
                                        originalProfile.City = userProfileEdit.City;
                                        originalProfile.DOB = userProfileEdit.DOB;
                                        originalProfile.Profile_Img_Path = New_Img_Path;
                                        originalProfile.Nationality_ID = userProfileEdit.Nationality_ID;

                                        _dbContext.Users.Update(originalProfile);
                                        await _dbContext.SaveChangesAsync();

                                        //get all current achievement for this user
                                        var currentAchievements = await _dbContext.AthleteAchievement.Where(w => w.Id == userProfileEdit.Id).ToListAsync();
                                        //find the new achievements need to insert
                                        var newAchievementToInsert = userProfileEdit.AthleteAchievements.Where(w => !currentAchievements.Select(s => s.Achievement).Contains(w))
                                            .Select(s => new AthleteAchievement
                                            {
                                                Id = userProfileEdit.Id,
                                                Achievement = s
                                            })
                                            .ToList();

                                        await _dbContext.AthleteAchievement.AddRangeAsync(newAchievementToInsert);
                                        await _dbContext.SaveChangesAsync();

                                        // get all achievements that need to be removed
                                        currentAchievements.RemoveAll(x => userProfileEdit.AthleteAchievements.Contains(x.Achievement));
                                        var achievementsToRemove = currentAchievements;

                                        _dbContext.AthleteAchievement.RemoveRange(achievementsToRemove);
                                        await _dbContext.SaveChangesAsync();

                                        transaction.Commit();

                                        ReturnResult = true;
                                    }
                                }
                                catch (DbUpdateConcurrencyException ex)
                                {
                                    var exceptionEntry = ex.Entries.Single();
                                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                                    if (databaseEntry == null)
                                    {
                                        ModelState.AddModelError(string.Empty,
                                            "Unable to save changes. The department was deleted by another user.");
                                    }
                                    else
                                    {
                                        var databaseValues = (AppUser)databaseEntry.ToObject();

                                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                                + "was modified by another user after you got the original value. The "
                                                + "edit operation was canceled. If you still want to edit this record, click "
                                                + "the Save button again.");

                                        userProfileEdit.RowVersion = (byte[])databaseValues.RowVersion;
                                        ModelState.Remove("RowVersion");
                                    }
                                    ReturnResult = false;
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Unable to save changes. " +
                                       "Try again, and if the problem persists, " +
                                       "see your system administrator.");

                                ReturnResult = false;
                            }
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", "Unable to save changes. " +
                                   "Try again, and if the problem persists, " +
                                   "see your system administrator.");

                            ReturnResult = false;
                        }
                    }
                }

                if (ReturnResult)
                {
                    TempData["Current_Uid"] = userProfileEdit.Id;

                    return RedirectToAction("GetCurrentUserProfile");
                }
                else
                {
                    // resign value from db, in case some user changed readonly value on html
                    userProfileEdit.Gender = originalProfile.Gender;
                    userProfileEdit.Email = originalProfile.Email;

                    return View(userProfileEdit);
                }
            }
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
                                  ActionExecutionDelegate next)
        {

            _jwtHandler.SetToken(context.HttpContext.Request.Cookies["access_token"]);//

            var result = await _jwtHandler.RoleIsUpdated();

            if (_jwtHandler.ExpiredInTenMins() || result)
            {
                var claims = _jwtHandler.GetClaims();

                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:privatekey"]));

                int expMins = Convert.ToInt32(_configuration["jwt:exp"]);

                var access_token = await _jwtHandler.RefreshToken(signInKey, expMins, claims);

                context.HttpContext.Response.Cookies.Append("access_token", access_token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });
            }

            _claim = _jwtHandler.GetClaims();

            ViewData["user_img_path"] = _claim.Img_Path;

            if (_claim.Role == "Admin")
            {
                ViewData["athletes_num"] = _dbContext.Users.ToList().Count();
            }
            else if (_claim.Role == "Organization")
            {
                var org_ids = _dbContext.Organization_Relationship.Where(w => w.Id == _claim.Uid && w.Role == "Organization Manager").Select(s => s.Org_ID).ToList();

                ViewData["teams_num"] = _dbContext.Team.Include(i => i.Organization).Where(w => org_ids.Contains(w.Organization.Org_ID)).Count();
                ViewData["athletes_num"] = _dbContext.Organization_Relationship.Include(i => i.Organization).Where(w => org_ids.Contains(w.Organization.Org_ID)).Select(s => s.Id).Distinct().Count();
            }
            else if (_claim.Role == "Manager")
            {
                var team_ids = _dbContext.Team_Membership.Where(w => w.Role == "Team Manager" && w.Position == "Team Manager" && w.Id == _claim.Uid).Include(inc => inc.Team).Select(s => s.Team.Team_ID).Distinct().ToList();
                ViewData["teams_num"] = team_ids.Count();
                ViewData["athletes_num"] = _dbContext.Team_Membership.Where(w => team_ids.Contains(w.Team_ID)).Select(s => s.Id).Distinct().Count();
            }
            else if (_claim.Role == "Athlete")
            {
                var best_score = _dbContext.Combine_Result.Where(x => x.Id == _claim.Uid).GroupBy(y => y.Point).Select(z => z.Max(c => c.Point));

                ViewData["best_combine_score"] = (best_score.ToList().Count == 0) ? 0 : best_score.First();
            }

            if (_claim.FirstName.Length >= 14)
            {
                ViewData["FirstName"] = _claim.FirstName.Substring(0, 14) + "...";
                ViewData["LastName"] = "";
            }
            else if ((_claim.FirstName + " " + _claim.FirstName).Length >= 14)
            {
                ViewData["FirstName"] = _claim.FirstName;
                ViewData["LastName"] = "";
            }
            else
            {
                ViewData["FirstName"] = _claim.FirstName;

                ViewData["LastName"] = _claim.LastName;
            }
            await next(); // the actual action

            // logic after the action goes here
        }
    }
}
