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
    public class EventInfoController : CustomizeController
    {

        public EventInfoController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> EventInfo(int id)
        {
            if (id == 0)
            {
                id = int.Parse(TempData["Event_ID"].ToString());
            }

            Event eve = await _dbContext.Event.FindAsync(id);

            var combine = _dbContext.Combine.Find(_dbContext.Event_Builder.Where(x => x.E_ID == eve.E_ID).First().C_ID);

            ViewData["Combine"] = combine;
            TempData["Event_ID"] = id;

            return View(eve);
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetMoreEventAttendees(int event_id, int last_attendee_id)
        {
            EventAttendeesList eventAttendeesList = new EventAttendeesList();

            var role = _claim.Role;

            var num = int.Parse(_configuration["NumbersOfEventAttendeesList:Value"]);

            eventAttendeesList.AdminAccess = role == "Admin";

            var eve = _dbContext.Event.Where(w => w.E_ID == event_id).FirstOrDefault();

            var now = DateTime.Now;

            eventAttendeesList.NotPastEvent = (DateTime.Compare(eve.Date.Date, now.Date) > 0) ? true : ((DateTime.Compare(eve.Date.Date, now.Date) == 0) ? ((TimeSpan.Compare(eve.Time.TimeOfDay, now.TimeOfDay) > 0) ? true : false) : false);

            eventAttendeesList.Attendees = _dbContext.Event_Assigned_Attendee.Where(w => w.E_ID == event_id).Select(s => s.AppUser).Where(x => x.Id > last_attendee_id).Select(v => new AppUser
            {
                Id = v.Id,
                FirstName = v.FirstName,
                LastName = v.LastName,
                Gender = v.Gender,
                DOB = v.DOB,
                Profile_Img_Path = (v.Profile_Img_Path == null) ? _configuration["UserProfileIconNullString:Value"] : v.Profile_Img_Path
            }).Take(num).ToList();


            return Content(JsonConvert.SerializeObject(eventAttendeesList), "application/json");
        }


        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> GetEventAttendeeDetail(int id)
        {
            List<AppUser> list = new List<AppUser>();
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Profile_Img_Path = (user.Profile_Img_Path == null || user.Profile_Img_Path == "") ? _configuration["UserProfileIconNullString:Value"] : user.Profile_Img_Path;

            return Content(JsonConvert.SerializeObject(new { User = user, AdminAccess = _claim.Role == "Admin" }), "application/json");
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> AssignEventAttendee(int id)
        {
            var eve = _dbContext.Event.Where(w => w.E_ID == id).FirstOrDefault();
            var now = DateTime.Now;
            var IsPastEvent = (DateTime.Compare(eve.Date.Date, now.Date) > 0) ? false
            : ((DateTime.Compare(eve.Date.Date, now.Date) == 0) ?
            ((TimeSpan.Compare(eve.Time.TimeOfDay, now.TimeOfDay) > 0) ? false : true) : true);

            if (IsPastEvent)
            {
                return Content(JsonConvert.SerializeObject(new { Result = false, Reason = "Refused by System, Cannot assign any attendee to a past event." }), "application/json");
            }
            else
            {
                TempData["Event_Name"] = (await _dbContext.Event.FindAsync(id) as Event).Name;
                TempData["Event_ID"] = id;
                return View();
            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> AssignEventAttendee(EventAttendeeBuilder eventAttendeeBuilder, int event_id)
        {
            if (!ModelState.IsValid)
            {
                var InValidDueToEmptyInput = false;
                foreach (var x in eventAttendeeBuilder.Members_Full)
                {
                    if (x.Id == 0 || x.FirstName == "" || x.LastName == "")
                    {
                        InValidDueToEmptyInput = true;
                        break;
                    }
                }
                if (InValidDueToEmptyInput)
                {
                    ModelState.AddModelError("", "You must full all input or remove the empty input field.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                           "Try again, and if the problem persists, " +
                           "see your system administrator.");
                }

                TempData["Event_Name"] = (await _dbContext.Event.FindAsync(event_id) as Event).Name;
                TempData["Event_ID"] = event_id;
                return View(eventAttendeeBuilder);
            }
            else
            {
                var eve = _dbContext.Event.Where(w => w.E_ID == event_id).FirstOrDefault();
                var now = DateTime.Now;
                var IsPastEvent = (DateTime.Compare(eve.Date.Date, now.Date) > 0) ? false
                : ((DateTime.Compare(eve.Date.Date, now.Date) == 0) ?
                ((TimeSpan.Compare(eve.Time.TimeOfDay, now.TimeOfDay) > 0) ? false : true) : true);

                if (IsPastEvent)
                {
                    ModelState.AddModelError("", "You must full all input or remove the empty input field.");
                    TempData["Event_Name"] = (await _dbContext.Event.FindAsync(event_id) as Event).Name;
                    TempData["Event_ID"] = event_id;
                    return View(eventAttendeeBuilder);
                }
                else
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {

                        foreach (var x in eventAttendeeBuilder.Members_Full)
                        {
                            _dbContext.Event_Assigned_Attendee.Update(new Event_Assigned_Attendee { E_ID = event_id, Id = x.Id });
                            _dbContext.SaveChanges();
                        }

                        transaction.Commit();

                        TempData["Event_ID"] = event_id;

                        return RedirectToAction("EventInfo", event_id);
                    }
                }
            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> RemoveEventAttendee(int id, int event_id)
        {
            var now = DateTime.Now;
            var eve = await _dbContext.Event.Where(e => e.E_ID == event_id).FirstOrDefaultAsync();

            if ((DateTime.Compare(eve.Date.Date, now.Date) > 0) ? false
                : ((DateTime.Compare(eve.Date.Date, now.Date) == 0) ?
                ((TimeSpan.Compare(eve.Time.TimeOfDay, now.TimeOfDay) > 0) ? false : true) : true))
            {
                return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Cannot remove any attendee from a past event." }), "application/json");
                //changing the event happened before is not allowed
            }
            else
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    var eventAttendee = await _dbContext.Event_Assigned_Attendee.Where(x => x.E_ID == event_id && x.Id == id).FirstOrDefaultAsync();

                    try
                    {
                        _dbContext.Event_Assigned_Attendee.Remove(eventAttendee);
                        _dbContext.SaveChanges();

                        transaction.Commit();
                        //only when commit successful, then return true, in case of any problem when transaction committed

                        return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Success." }), "application/json");
                    }
                    catch (Exception e)
                    {
                        return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Failed remove attendee, please try again. If keeps happen, contact our administrator." }), "application/json");
                    }

                }
            }
        }
    }
}
