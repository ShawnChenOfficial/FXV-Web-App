using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class OrganizationInfoController : CustomizeController
    {
        public OrganizationInfoController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> OrganizationInfo(int id)
        {
            if (id == 0)
            {
                id = int.Parse(TempData["Org_ID"].ToString());
            }

            // ensure if user manually changes the org ID on html to view team information that is not supposed to be available to them.
            if (_claim.Role != "Admin")
            {
                if (!HasAccessToViewOrgInfo(id))
                {
                    return RedirectToAction("Forbid_403", "Errors");
                }
            }

            ViewModel_Organization org = await _dbContext.Organization
                                    .Where(w => w.Org_ID == id).Include(inc => inc.AppUser)
                                    .Select(s => new ViewModel_Organization
                                    {
                                        OrgId = s.Org_ID,
                                        Location = s.Location,
                                        Name = s.Name,
                                        Description = s.Description,
                                        NumberOfTeams = s.Num_Of_Teams,
                                        NumberOfMembers = _dbContext.Organization_Relationship.Where(ww => ww.Org_ID == s.Org_ID).Count(),
                                        ImgPath = s.Img_Path,
                                        ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName
                                    }).FirstOrDefaultAsync();
            return View(org);
        }

        //
        //This method is the get method used for create team
        //
        [Authorize("AdminOrganization")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> Create(int org_id)
        {
            if (_claim.Role != "Admin" && !await IsRelevantOrgManager(org_id))
            {
                return RedirectToAction("Forbid_403", "Error");
            }
            TempData["Org_ID"] = org_id;
            return View();
        }

        //
        //This method is the post method used for create team
        //
        [Authorize("AdminOrganization")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Create(TeamBuilder teamBuilder, int _org_id)
        {
            if (!ModelState.IsValid)
            {
                var __org_id = _org_id;
                TempData["Org_ID"] = __org_id;
                return View(teamBuilder);
            }
            else if (_claim.Role != "Admin" && !await IsRelevantOrgManager(_org_id))
            {
                return RedirectToAction("Forbid_403", "Error");
            }
            else
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var Img_Path = "";

                        if (teamBuilder.Image != null)
                        {
                            var date = Request;
                            var files = Request.Form.Files;
                            long size = files.Sum(f => f.Length);
                            string contentRootPath = _hostingEnvironment.ContentRootPath;
                            IFormFile img = teamBuilder.Image;


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
                                var filePath = webRootPath + "./sources/teamImg/" + newFileName;
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await img.CopyToAsync(stream);
                                }
                                Img_Path = "./sources/teamImg/" + newFileName;
                            }
                        }
                        else if (teamBuilder.Image == null)
                        {
                            Img_Path = _configuration["TeamImgNullString:Value"];
                        }

                        // save team
                        var team = new Team
                        {
                            Name = teamBuilder.Name,
                            Location = teamBuilder.Location,
                            Description = teamBuilder.Description,
                            Id = teamBuilder.Owner.Id,
                            Org_ID = _org_id,
                            Img_Path = Img_Path
                        };

                        _dbContext.Team.Update(team);
                        _dbContext.SaveChanges();

                        // save manager in team membership table
                        var team_Memberships = new Team_Membership
                        {
                            Team_ID = team.Team_ID,
                            Id = team.Id,
                            Role = "Team Manager",
                            Position = "Team Manager"
                        };

                        _dbContext.Team_Membership.Update(team_Memberships);
                        _dbContext.SaveChanges();

                        // assign team manager in org membership table
                        var manager_in_org = _dbContext.Organization_Relationship.Where(w => w.Org_ID == _org_id && w.Id == teamBuilder.Owner.Id).FirstOrDefault();

                        if (manager_in_org == null)
                        {
                            manager_in_org = new Organization_Relationship
                            {
                                Id = teamBuilder.Owner.Id,
                                Org_ID = _org_id,
                                Role = "Team Manager"
                            };

                            await _dbContext.Organization_Relationship.AddAsync(manager_in_org);
                            await _dbContext.SaveChangesAsync();
                        }
                        else if (manager_in_org.Role != "Organization Manager")
                        {
                            manager_in_org.Role = "Team Manager";

                            await _dbContext.Organization_Relationship.AddAsync(manager_in_org);
                            await _dbContext.SaveChangesAsync();
                        }

                        // change the team manager as the system manager
                        var user = await _userManager.FindByIdAsync(team_Memberships.Id.ToString());

                        if (await _userManager.IsInRoleAsync(user, "Athlete"))
                        {
                            await _userManager.RemoveFromRoleAsync(user, "Athlete");
                            await _userManager.AddToRoleAsync(user, "Manager");
                            await _dbContext.SaveChangesAsync();
                        }

                        // save team members in team membership and org membership
                        if (teamBuilder.Members != null)
                        {

                            foreach (var x in teamBuilder.Members)
                            {
                                if (x.Member != null && x.Member.Id != 0)
                                {
                                    if (x.Member.Id != team.Id)
                                    {
                                        team_Memberships = new Team_Membership
                                        {
                                            Team_ID = team.Team_ID,
                                            Id = x.Member.Id,
                                            Role = "Athlete",
                                            Position = "Athlete"
                                        };

                                        _dbContext.Team_Membership.Update(team_Memberships);
                                        _dbContext.SaveChanges();

                                        if (!_dbContext.Organization_Relationship.Any(y => y.Org_ID == _org_id && y.Id == x.Member.Id))
                                        {
                                            _dbContext.Organization_Relationship
                                                .Update(new Organization_Relationship
                                                {
                                                    Org_ID = _org_id,
                                                    Id = x.Member.Id,
                                                    Role = "Athlete"
                                                }
                                            );

                                            _dbContext.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }

                        // update the number of team members and teams
                        team.Num_Of_Members = _dbContext.Team_Membership.Where(y => y.Team_ID == team.Team_ID).ToList().Count;
                        _dbContext.Team.Update(team);
                        _dbContext.SaveChanges();

                        var org = await _dbContext.Organization.FindAsync(_org_id);
                        org.Num_Of_Teams = _dbContext.Team.Where(y => y.Org_ID == team.Org_ID).ToList().Count;

                        _dbContext.Organization.Update(org);
                        _dbContext.SaveChanges();

                        TempData["org_id"] = _org_id;

                        transaction.Commit();

                        return RedirectToAction("OrganizationInfo", _org_id);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                                           "Try again, and if the problem persists, " +
                                           "see your system administrator.");

                        TempData["Org_ID"] = _org_id;
                        return View(teamBuilder);
                    }
                }
            }
        }


        //
        //This method is used to edit organization
        //
        [Authorize("AdminOrganization")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Edit(int org_id)
        {
            var HasAccess = _dbContext.Organization_Relationship.Where(w => w.Org_ID == org_id && w.Id == _claim.Uid).Count() > 0;

            if (_claim.Role != "Admin" && !HasAccess)
            {
                //return RedirectToAction("Forbid_403", "Errors");
                return Content(JsonConvert.SerializeObject(new { Result = false, Reason = "Access denied, you do not have access to edit this Organization." }), "application/json");
            }


            var org = _dbContext.Organization.Where(w => w.Org_ID == org_id).Include(inc => inc.AppUser).FirstOrDefault();

            if (org == null)
            {
                return Content(JsonConvert.SerializeObject(new { Result = false, Reason = "Organization has been deleted" }), "application/json");
            }

            OrganizationBuilder organizationBuilder = new OrganizationBuilder
            {
                Org_ID = org.Org_ID,
                Name = org.Name,
                Description = org.Description,
                Location = org.Location,
                Manager_Name = org.AppUser.FirstName + " " + org.AppUser.LastName,
                Owner = org.AppUser,
                Image_Path = org.Img_Path,
                RowVersion = org.RowVersion
            };

            return View(organizationBuilder);
        }

        //
        //This method is a post method used to edit organization
        //
        [Authorize("AdminOrganization")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Edit(OrganizationBuilder organizationBuilder)
        {
            var ReturnResult = false;

            var originalOrg = await _dbContext.Organization.Where(x => x.Org_ID == organizationBuilder.Org_ID).Include(inc => inc.AppUser).FirstOrDefaultAsync();

            if (!ModelState.IsValid)
            {
                ReturnResult = false;
            }
            else
            {
                if (originalOrg == null)
                {
                    ModelState.AddModelError("", "Unable to save changes. The Organization was deleted by another user");
                    ReturnResult = false;
                }

                try
                {
                    var New_Img_Path = originalOrg.Img_Path;

                    if (organizationBuilder.Image != null)
                    {
                        var date = Request;
                        var files = Request.Form.Files;
                        long size = files.Sum(f => f.Length);
                        string contentRootPath = _hostingEnvironment.ContentRootPath;
                        IFormFile img = organizationBuilder.Image;


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
                            var filePath = webRootPath + "./sources/orgImg/" + newFileName;
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await img.CopyToAsync(stream);
                            }
                            New_Img_Path = "./sources/orgImg/" + newFileName;
                        }
                    }

                    var entry = _dbContext.Entry(originalOrg);

                    entry.Property("RowVersion").OriginalValue = organizationBuilder.RowVersion;

                    if (await TryUpdateModelAsync<Organization>(originalOrg, "", x => x.Name, x => x.Description, x => x.Location, x => x.Img_Path))
                    {
                        try
                        {
                            using (var transaction = _dbContext.Database.BeginTransaction())
                            {
                                originalOrg.Img_Path = New_Img_Path;
                                originalOrg.Name = organizationBuilder.Name;
                                originalOrg.Description = organizationBuilder.Description;
                                originalOrg.Location = organizationBuilder.Location;

                                _dbContext.Organization.Update(originalOrg);

                                await _dbContext.SaveChangesAsync(); //the error is coming from here

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
                                var databaseValues = (Organization)databaseEntry.ToObject();

                                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                        + "was modified by another user after you got the original value. The "
                                        + "edit operation was cancelled. If you still want to edit this record, click "
                                        + "the Save button again. Otherwise click the Back to List all Organizations.");

                                organizationBuilder.RowVersion = (byte[])databaseValues.RowVersion;

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
                               "see your system administrator." + e);

                    ReturnResult = false;
                }
            }

            if (ReturnResult)
            {
                return View("Organizations");
            }
            else
            {
                // just in case of some user change the value of iteams which are not supposed to be allowed to change on html, then return them the correct value
                organizationBuilder.Manager_Name = originalOrg.AppUser.FirstName + " " + originalOrg.AppUser.LastName;
                organizationBuilder.Owner = originalOrg.AppUser;
                return View(organizationBuilder);
            }
        }

        //
        //This method is used to remove organization
        //
        [Authorize("AdminOrganization")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var org = _dbContext.Organization.Find(id);

            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    var org_manager_ids = _dbContext.Organization_Relationship.Where(w => w.Org_ID == id && w.Role == "Organization Manager").Select(s => s.Id).ToList();

                    org_manager_ids.RemoveAll(x => _dbContext.Organization_Relationship.Where(w => w.Org_ID != id && w.Id == x && w.Role == "Organization Manager").Count() > 0);

                    //find all team manager from the org manager id list
                    var SystemPermissionToManager = _dbContext.Team_Membership.Where(w => w.Role == "Team Manager" && org_manager_ids.Contains(w.Id)).Include(inc => inc.AppUser).Select(s => s.AppUser).Distinct().ToList();
                    // remove all this org manager who are also the team manager as well from the list, to get the list of org manager that need to be changed to normal athlete permission
                    org_manager_ids.RemoveAll(x => _dbContext.Team_Membership.Where(w => w.Role == "Team Manager" && w.Id == x).Include(inc => inc.Team).Where(ww => ww.Team.Org_ID != 0).Select(s => s.Id).Count() > 0);

                    var SystemPermissionToAthlete = _dbContext.Users.Where(w => org_manager_ids.Contains(w.Id));


                    foreach (var x in SystemPermissionToAthlete)
                    {
                        if ((await _userManager.GetRolesAsync(x))[0] != "Admin")
                        {
                            await _userManager.RemoveFromRoleAsync(x, "Organization");
                            await _userManager.AddToRoleAsync(x, "Athlete");
                            await _dbContext.SaveChangesAsync();
                        }
                    }

                    foreach (var x in SystemPermissionToManager)
                    {
                        if ((await _userManager.GetRolesAsync(x))[0] != "Admin")
                        {
                            await _userManager.RemoveFromRoleAsync(x, "Organization");
                            await _userManager.AddToRoleAsync(x, "Manager");
                            await _dbContext.SaveChangesAsync();
                        }
                    }

                    var teams = _dbContext.Team.Where(w => w.Org_ID == org.Org_ID).ToList();

                    foreach (var x in teams)
                    {
                        x.Org_ID = 0;

                        _dbContext.Team.Update(x);
                        _dbContext.SaveChanges();
                    }

                    _dbContext.Organization.Remove(org);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Success" }), "application/json");
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed to remove organization, please try again. If keeps happen, contact our administrator." }), "application/json");
            }
        }
    }
}
