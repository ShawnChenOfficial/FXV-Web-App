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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class OrganizationTeamInfoController : CustomizeController
    {

        public OrganizationTeamInfoController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> OrganizationTeamInfo(int id)
        {
            if (id == 0)
            {
                id = int.Parse(TempData["Team_ID"].ToString());
            }

            // ensure if user manually changes the team ID on html to view team information that is not supposed to be available to them.
            if (_claim.Role != "Admin")
            {
                if (!HasAccessToTeam(id))
                {
                    return RedirectToAction("Forbid_403", "Errors");
                }
            }

            var team = await _dbContext.Team.Where(w => w.Team_ID == id)
                .Include(inc => inc.AppUser)
                .Select(s => new ViewModel_Team
                {
                    TeamId = s.Team_ID,
                    OrgId = s.Org_ID,
                    Name = s.Name,
                    Description = s.Description,
                    Location = s.Location,
                    Img_Path = s.Img_Path,
                    ManagerName = s.AppUser.FirstName + " " + s.AppUser.LastName,
                    NumberOfMembers = s.Num_Of_Members

                }).FirstOrDefaultAsync();

            return View(team);
        }

        //
        //This method is the get method used for edit team
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> Edit(int team_id)
        {
            var originalTeam = await _dbContext.Team.Where(w => w.Team_ID == team_id).Include(inc => inc.AppUser).FirstOrDefaultAsync();

            if (_claim.Role != "Admin" && !await IsRelevantOrgManager(originalTeam.Org_ID) && !IsRelevantTeamManager(team_id))
            {
                return RedirectToAction("Forbid_403", "Errors");
            }
            else if (originalTeam == null)
            {
                return Content(JsonConvert.SerializeObject(new { Result = false, Reason = "The team " + originalTeam.Name + " has been deleted by other user." }), "application/json");
            }
            else
            {
                TeamBuilder teamBuilder = new TeamBuilder
                {
                    Team_ID = originalTeam.Team_ID,
                    Name = originalTeam.Name,
                    Description = originalTeam.Description,
                    Img_Path = originalTeam.Img_Path,
                    Location = originalTeam.Location,
                    Manager_Name = originalTeam.AppUser.FirstName + " " + originalTeam.AppUser.LastName,
                    Owner = originalTeam.AppUser,
                    Members = await _dbContext.Team_Membership.Where(w => w.Team_ID == team_id).Include(inc => inc.AppUser).Select(s => new TeamMember { Member = new Member { Id = s.AppUser.Id, FullName = (s.Role == "Team Manager") ? s.AppUser.FirstName + " " + s.AppUser.LastName + "(" + s.Role + ")" : s.AppUser.FirstName + " " + s.AppUser.LastName }, Role = s.Role }).ToListAsync(),
                    RowVersion = originalTeam.RowVersion
                };

                // re-sign org_id, but same value (catering to different memory address in case of data lost by end of this method.)
                TempData["Org_ID"] = originalTeam.Org_ID;

                return View(teamBuilder);
            }
        }

        //
        //This method is the post method used for edit team
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Edit(TeamBuilder teamBuilder, int org_id)
        {
            var ReturnResult = false;

            var team_id = teamBuilder.Team_ID;

            var originalTeam = await _dbContext.Team.Where(w => w.Team_ID == team_id).Include(inc => inc.AppUser).FirstOrDefaultAsync();

            var _org_id = originalTeam.Org_ID;

            if (_claim.Role != "Admin" && !await IsRelevantOrgManager(_org_id) && !IsRelevantTeamManager(team_id))
            {
                return RedirectToAction("Forbid_403", "Errors");
            }
            // just in case of if someone manually change the id on html, otherwise it will break the relationships between orgs and teams
            else if (org_id != _org_id)
            {
                return RedirectToAction("Forbid_403", "Errors");
            }
            else
            {
                if (originalTeam == null)
                {
                    ModelState.AddModelError("", "Unable to save changes. The team was deleted by another user.");
                    ReturnResult = false;
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        ReturnResult = false;
                    }
                    else
                    {
                        try
                        {
                            var New_Img_Path = originalTeam.Img_Path;

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
                                    New_Img_Path = "./sources/teamImg/" + newFileName;
                                }
                            }


                            var entry = _dbContext.Entry(originalTeam);

                            entry.Property("RowVersion").OriginalValue = teamBuilder.RowVersion;

                            if (await TryUpdateModelAsync<Team>(originalTeam, "", e => e.Name, e => e.Description, e => e.Img_Path, e => e.Id, e => e.Img_Path))
                            {
                                try
                                {
                                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                                    {

                                        originalTeam.Name = teamBuilder.Name;
                                        originalTeam.Description = teamBuilder.Description;
                                        originalTeam.Location = teamBuilder.Location;
                                        originalTeam.Img_Path = New_Img_Path;

                                        _dbContext.Team.Update(originalTeam);
                                        await _dbContext.SaveChangesAsync();


                                        // get current team members
                                        var currentTeamMembers = await _dbContext.Team_Membership.Where(w => w.Team_ID == team_id).ToListAsync();

                                        var newTeamMembersToInsert = teamBuilder.Members.Where(w => !currentTeamMembers.Select(s => s.Id).Contains(w.Member.Id) && w.Member.Id != 0)
                                                                        .Select(ss => new Team_Membership
                                                                        {
                                                                            Team_ID = team_id,
                                                                            Id = ss.Member.Id,
                                                                            Role = "Athlete",
                                                                            Position = "Athlete"
                                                                        }).ToList();

                                        await _dbContext.Team_Membership.AddRangeAsync(newTeamMembersToInsert);
                                        await _dbContext.SaveChangesAsync();

                                        // get all team members removed
                                        currentTeamMembers.RemoveAll(x => teamBuilder.Members.Select(s => s.Member.Id).Contains(x.Id));

                                        var teamMembersToRemove = currentTeamMembers;

                                        foreach (var x in teamMembersToRemove)
                                        {
                                            if (x.Role == "Team Manager" && x.Position == "Team Manager")
                                            {
                                                ModelState.AddModelError("", "Team Manager cannot be removed while edit a Team");

                                                ViewData["Org_ID"] = _org_id;

                                                teamBuilder.Members = await _dbContext.Team_Membership.Where(w => w.Team_ID == team_id).Select(s => new TeamMember { Member = new Member { Id = s.AppUser.Id, FullName = s.AppUser.FirstName + " " + s.AppUser.LastName }, Role = s.Role }).ToListAsync();

                                                return View(teamBuilder);
                                            }
                                        }

                                        _dbContext.Team_Membership.RemoveRange(teamMembersToRemove);
                                        await _dbContext.SaveChangesAsync();

                                        // check if need to add any team member to organization relationship table as well
                                        // get all current memeberships in same org
                                        var currentMembershipInSameOrg = await _dbContext.Organization_Relationship.Where(w => newTeamMembersToInsert.Select(s => s.Id).Contains(w.Id) && w.Org_ID == _org_id).ToListAsync();
                                        // remove all added team members who have associated with other team in same org already from added team members list.
                                        newTeamMembersToInsert.RemoveAll(x => currentMembershipInSameOrg.Select(s => s.Id).Contains(x.Id));
                                        var newOrgMemberToInsert = newTeamMembersToInsert.Select(s => new Organization_Relationship { Id = s.Id, Org_ID = _org_id, Role = "Athlete" }).ToList();

                                        await _dbContext.Organization_Relationship.AddRangeAsync(newOrgMemberToInsert);
                                        await _dbContext.SaveChangesAsync();


                                        // check if need to remove any member from organization relationship table.
                                        // check if any removed team members also associated with OTHER team under the same org. (exclude edited team).
                                        var allCurrentTeamMembersInSameOrgExcludeThisTeam = await _dbContext.Team_Membership.Include(inc => inc.Team).Where(w => w.Team.Org_ID == _org_id && w.Team_ID != team_id).ToListAsync();
                                        // remove all athletes who have associated with other team in same org from the removed team member list
                                        teamMembersToRemove.RemoveAll(x => allCurrentTeamMembersInSameOrgExcludeThisTeam.Select(s => s.Id).Contains(x.Id));
                                        var OrgMemberToRemove = await _dbContext.Organization_Relationship.Where(w => w.Org_ID == _org_id && teamMembersToRemove.Select(s => s.Id).Contains(w.Id)).ToListAsync();

                                        _dbContext.Organization_Relationship.RemoveRange(OrgMemberToRemove);
                                        await _dbContext.SaveChangesAsync();

                                        // update the current number of team members for this team.
                                        originalTeam.Num_Of_Members = _dbContext.Team_Membership.Where(w => w.Team_ID == team_id).Count();

                                        _dbContext.Team.Update(originalTeam);
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
                                            "Unable to save changes. The team was deleted by another user.");
                                    }
                                    else
                                    {
                                        var databaseValues = (Team)databaseEntry.ToObject();

                                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                                + "was modified by another user after you got the original value. The "
                                                + "edit operation was canceled. If you still want to edit this record, click "
                                                + "the Save button again.");

                                        teamBuilder.RowVersion = (byte[])databaseValues.RowVersion;
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
                    TempData["Org_ID"] = _org_id;
                    return RedirectToAction("OrganizationInfo");
                }
                else
                {
                    // just in case of some user change the value of iteams which are not supposed to be allowed to change on html, then return them the correct value
                    teamBuilder.Manager_Name = originalTeam.AppUser.FirstName + " " + originalTeam.AppUser.LastName;
                    teamBuilder.Owner = originalTeam.AppUser;
                    TempData["Org_ID"] = _org_id;
                    return View(teamBuilder);
                }
            }
        }

        //
        //This method is the post method used for remove a team
        //
        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            // need to check does the current user has privilege to do this.

            var HasAccess = _dbContext.Organization_Relationship.Where(w => w.Id == _claim.Uid && w.Role == "Organization Manager" && w.Org_ID == _dbContext.Team.Where(ww => ww.Team_ID == id).Select(s => s.Org_ID).FirstOrDefault()).Count() > 0;

            if (_claim.Role != "Admin" && !HasAccess)
            {
                // either way to return the error message

                //return RedirectToAction("Forbid_403", "Errors");
                return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Access denied, you do not have access to remove this Team." }), "application/json");
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var org_id = _dbContext.Team.Find(id).Org_ID;

                    var Members_In_This_Team = _dbContext.Team_Membership.Where(w => w.Team_ID == id).Select(s => s.Id).ToList();
                    var Members_In_Org_Not_In_This_Team = _dbContext.Team_Membership.Include(inc => inc.Team).Where(w => w.Team_ID != id && w.Team.Org_ID == org_id).Select(s => s.Id).Distinct().ToList();
                    var MembersToRemoveFromOrg = Members_In_This_Team.Where(w => !Members_In_Org_Not_In_This_Team.Contains(w)).ToList();

                    foreach (var x in MembersToRemoveFromOrg)
                    {
                        var rowToRemove = _dbContext.Organization_Relationship.Where(w => w.Id == x && w.Org_ID == org_id && w.Role != "Organization Manager").FirstOrDefault();
                        if (rowToRemove != null)
                        {
                            _dbContext.Organization_Relationship.Remove(rowToRemove);
                            _dbContext.SaveChanges();
                        }
                    }


                    //set the Org_Id to 0 in team table, in order to keep just break the relationship between org and team, then remain the team in the system.
                    var team = await _dbContext.Team.Where(t => t.Team_ID == id).FirstOrDefaultAsync();
                    team.Org_ID = 0;  //set team's Org_ID to 0 which is a Invisible org call Invisible teams collection
                    _dbContext.Team.Update(team);
                    await _dbContext.SaveChangesAsync();

                    //get all team manager user ids to check if we need to downward their system permission
                    var Manager_ids_In_This_Team = _dbContext.Team_Membership.Where(w => w.Team_ID == id && w.Role == "Team Manager").Select(s => s.Id).ToList(); // find all team manager in this team

                    Manager_ids_In_This_Team.RemoveAll(r => _dbContext.Team_Membership.Where(w => w.Team_ID != id && w.Role == "Team Manager" && w.Id == r).Include(inc => inc.Team).Where(ww => ww.Team.Org_ID != 0).Count() > 0);// remove the manager who are also other team's manager in the amount of manager just found in the current team going to be deleted

                    var SystemPermissionToAthlete = Manager_ids_In_This_Team; // get all team managers in the team which is about to delete, who are not other team's manager.

                    foreach (var x in SystemPermissionToAthlete) // then downward their system permission
                    {
                        var CurrentManager = await _dbContext.Users.FindAsync(x);

                        var CurrentManagerRole = (await _userManager.GetRolesAsync(CurrentManager))[0];

                        if (CurrentManagerRole != "Admin" && CurrentManagerRole != "Organization")
                        {
                            await _userManager.RemoveFromRoleAsync(CurrentManager, CurrentManagerRole);
                            await _userManager.AddToRoleAsync(CurrentManager, "Athlete");
                            await _dbContext.SaveChangesAsync();
                        }
                    }

                    var org = await _dbContext.Organization.Where(w => w.Org_ID == org_id).FirstOrDefaultAsync();
                    org.Num_Of_Teams = _dbContext.Team.Where(w => w.Org_ID == org_id).Count();
                    _dbContext.Organization.Update(org);
                    await _dbContext.SaveChangesAsync();
                    // re-coded by Shawn

                    transaction.Commit(); //only when commit successful, then return true, in case of any problem when transaction committed
                    return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Success." }), "application/json");
                }
                catch (Exception e)
                {
                    return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed to remove team, please try again. If keeps happen, contact our administrator." }), "application/json");
                }
            }
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> AddTeamMember(int team_id)
        {
            // ensure if user manually changes the team ID on html to view team information that is not supposed to be available to them.
            if (_claim.Role != "Admin")
            {
                if (!HasAccessToTeam(team_id))
                {
                    return RedirectToAction("Forbid_403", "Errors");
                }
            }

            ViewData["Team_Name"] = (await _dbContext.Team.FindAsync(team_id)).Name;
            TempData["Team_ID"] = team_id;
            return View();
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> AddTeamMember(OrgTeamMemberBuilder orgTeamMemberBuilder, int team_id)
        {
            // ensure if user manually changes the team ID on html to view team information that is not supposed to be available to them.
            if (_claim.Role != "Admin")
            {
                if (!HasAccessToTeam(team_id))
                {
                    return RedirectToAction("Forbid_403", "Errors");
                }
            }

            if (!ModelState.IsValid)
            {
                var InValidDueToEmptyInput = false;
                foreach (var x in orgTeamMemberBuilder.Members_Full_Users)
                {
                    if (x.Id == 0 || x.FirstName == "" || x.LastName == "")
                    {
                        InValidDueToEmptyInput = true;
                        break;
                    }
                }
                if (InValidDueToEmptyInput)
                {
                    ModelState.AddModelError("", "Plese fill all inputs by search box or remove useless input(s).");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                           "Try again, and if the problem persists, " +
                           "see your system administrator.");
                }

                ViewData["Team_Name"] = (await _dbContext.Team.FindAsync(team_id)).Name;
                TempData["Team_ID"] = team_id;
                return View(orgTeamMemberBuilder);
            }
            else
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var team = await _dbContext.Team.FindAsync(team_id);

                        foreach (var x in orgTeamMemberBuilder.Members_Full_Users)
                        {
                            if (x != null && x.Id != 0)
                            {
                                if (x.Id != team.Id)
                                {
                                    var team_Memberships = new Team_Membership
                                    {
                                        Team_ID = team_id,
                                        Id = x.Id,
                                        Role = "Athlete"
                                    };

                                    _dbContext.Team_Membership.Update(team_Memberships);
                                    _dbContext.SaveChanges();

                                    if (!_dbContext.Organization_Relationship.Any(y => y.Org_ID == team.Org_ID && y.Id == x.Id))
                                    {
                                        _dbContext.Organization_Relationship
                                                .Update(new Organization_Relationship
                                                {
                                                    Org_ID = team.Org_ID,
                                                    Id = x.Id,
                                                    Role = "Athlete"
                                                }
                                            );

                                        _dbContext.SaveChanges();
                                    }
                                }
                            }
                        }

                        team.Num_Of_Members = _dbContext.Team_Membership.Where(y => y.Team_ID == team.Team_ID).ToList().Count;
                        _dbContext.Team.Update(team);
                        _dbContext.SaveChanges();

                        var org = await _dbContext.Organization.FindAsync(team.Org_ID);
                        org.Num_Of_Teams = _dbContext.Team.Where(y => y.Org_ID == team.Org_ID).ToList().Count;

                        _dbContext.Organization.Update(org);
                        _dbContext.SaveChanges();

                        transaction.Commit();

                        ViewData["Team_Name"] = (await _dbContext.Team.FindAsync(team_id)).Name;
                        TempData["Team_ID"] = team_id;


                        return RedirectToAction("OrganizationTeamInfo", team_id);
                    }
                    catch (Exception e)
                    {

                        ModelState.AddModelError("", "Unable to save changes. " +
                                           "Try again, and if the problem persists, " +
                                           "see your system administrator.");

                        ViewData["Team_Name"] = (await _dbContext.Team.FindAsync(team_id)).Name;
                        ViewData["Team_ID"] = team_id;
                        return View(orgTeamMemberBuilder);
                    }
                }
            }

        }


        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> RemoveTeamMember(int id, int team_id)
        {
            //get which org is this team belonging to.
            var org_id = _dbContext.Team.Where(w => w.Team_ID == team_id).FirstOrDefault().Org_ID;

            // check if current user is the manager of relevant org or relevant team
            var HasAccessToTeam = _dbContext.Team_Membership.Where(w => w.Team_ID == team_id && w.Id == _claim.Uid && w.Role == "Team Manager" && w.Position == "Team Manager").Count() > 0 ||
                            _dbContext.Organization_Relationship.Where(w => w.Org_ID == org_id && w.Id == _claim.Uid && w.Role == "Organization Manager").Count() > 0 || _claim.Role == "Admin";

            if (HasAccessToTeam)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var team_member = await _dbContext.Team_Membership.Where(u => u.Team_ID == team_id && u.Id == id).FirstOrDefaultAsync();

                        if (team_member == null)
                        {
                            return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed remove team member, this Team member has been removed from team." }), "application/json");
                        }
                        else if (team_member.Role == "Team Manager")
                        {
                            return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Cannot remove a team manager from a team, please downward the permission to be Athlete first." }), "application/json");
                        }

                        _dbContext.Team_Membership.Remove(team_member);
                        _dbContext.SaveChanges();

                        var team = _dbContext.Team.Where(w => w.Team_ID == team_id).FirstOrDefault();
                        team.Num_Of_Members = _dbContext.Team_Membership.Where(w => w.Team_ID == team_id).Count();

                        _dbContext.Team.Update(team);
                        _dbContext.SaveChanges();

                        // check other teams under the same org
                        var other_team_ids_in_same_org = _dbContext.Team.Where(w => w.Org_ID == org_id && w.Team_ID != team_id).Select(s => s.Team_ID).ToList();
                        // check if athlete belongling to other team in same org
                        var athlete_in_other_team_in_same_org = _dbContext.Team_Membership.Where(w => w.Id == id && other_team_ids_in_same_org.Contains(w.Team_ID)).ToList();

                        //if athlete is not belonging to other team in same org
                        if (athlete_in_other_team_in_same_org.Count == 0)
                        {
                            var org_relationship = _dbContext.Organization_Relationship.Where(w => w.Org_ID == org_id && w.Id == id).FirstOrDefault();
                            _dbContext.Organization_Relationship.Remove(org_relationship);
                            _dbContext.SaveChanges();
                        }

                        transaction.Commit();

                        //only when commit successful, then return true, in case of any problem when transaction committed

                        return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Success" }), "application/json");
                    }
                    catch (Exception e)
                    {
                        return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed remove team member, please try again. If keeps happen, contact our administrator." }), "application/json");
                    }
                }
            }
            else
            {
                return RedirectToAction("Forbid_403","Error");
            }
        }
    }
}
