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
    public class EventsController : CustomizeController
    {
        public EventsController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Events()//loading the recommanded events which are decided to show by Admin
        {
            var today = DateTime.Now;

            List<Event> list = new List<Event>();

            //list = _dbContext.Events.Where(x => DateTime.Compare(x.Date.Date, today.Date) >= 0).OrderBy(x => x.Date).Take(5).ToList();
            list = _dbContext.Event.OrderBy(x => x.Date).Take(5).ToList();

            ViewData["List"] = list;
            return View();
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public IActionResult GetMoreEvents(int last_event_id) // loading all the events
        {
            var today = DateTime.Now;

            EventsList eventsList = new EventsList();

            var role = _claim.Role;

            var num = int.Parse(_configuration["NumbersOfEventsList:Value"]);

            eventsList.IsAdmin = role == "Admin";
            switch (role)
            {
                case "Admin":
                    eventsList.Events = _dbContext.Event.OrderByDescending(x => x.Date).Where(y => y.E_ID > last_event_id).Take(num).ToList();
                    break;
                case "Organization":
                    eventsList.Events = _dbContext.Event.OrderByDescending(x => x.Date).Where(y => y.E_ID > last_event_id).Take(num).ToList();
                    break;
                case "Manager":
                    eventsList.Events = _dbContext.Event.OrderByDescending(x => x.Date).Where(y => y.E_ID > last_event_id).Take(num).ToList();
                    break;
                case "Athlete":
                    eventsList.Events = _dbContext.Event.OrderByDescending(x => x.Date).Where(y => y.E_ID > last_event_id).Take(num).ToList();
                    break;
            }

            return Content(JsonConvert.SerializeObject(eventsList), "application/json");
        }
        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> GetEventDetails(int id)
        {
            List<Event> list = new List<Event>();

            Event eve = await _dbContext.Event.FindAsync(id);

            list.Add(eve);

            return Content(JsonConvert.SerializeObject(list), "application/json");
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> RemoveEvent(int e_id)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var list_event_result = _dbContext.Event_Result.Where(x => x.E_ID == e_id).ToList();
                    if (list_event_result.Count > 0)
                    {
                        return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed! This Event has related to one or more Event Result, please remove all relevant Event Result firstly." }), "application/json");
                    }
                    else
                    {
                        _dbContext.Event.Remove(await _dbContext.Event.FindAsync(e_id));
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Success." }), "application /json");
                    }
                }
                catch (Exception ex)
                {
                    return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Failed to remove combine, please try again. If keeps happen, contact our administrator." }), "application/json");
                }
            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Edit(int e_id)
        {
            EventBuilder eventBuilder = new EventBuilder();

            var eve = _dbContext.Event.Where(w => w.E_ID == e_id).FirstOrDefault();

            if (e_id == 0 && eve == null)
            {
                return Content(JsonConvert.SerializeObject(new { Result = false, Reason = "Event has been deleted." }), "application /json");
            }
            else
            {
                var now = DateTime.Now;

                eventBuilder.IsPastEvent = (DateTime.Compare(eve.Date.Date, now.Date) > 0) ? false
                : ((DateTime.Compare(eve.Date.Date, now.Date) == 0) ?
                ((TimeSpan.Compare(eve.Time.TimeOfDay, now.TimeOfDay) > 0) ? false : true) : true);

                if (eventBuilder.IsPastEvent)
                {
                    ModelState.AddModelError("", "As this is a past event, the started date, time, combine and attendees cannot be edited anymore.");
                }

                eventBuilder.RowVersion = eve.RowVersion;
                eventBuilder.E_ID = eve.E_ID;
                eventBuilder.Name = eve.Name;
                eventBuilder.Location = eve.Location;
                eventBuilder.Date = eve.Date;
                eventBuilder.Time = eve.Time;
                eventBuilder.Description = eve.Description;
                eventBuilder.Combine = _dbContext.Event_Builder.Where(w => w.E_ID == e_id).Include(inc => inc.Combine).Select(s => s.Combine).FirstOrDefault();
                eventBuilder.Attendees = _dbContext.Event_Assigned_Attendee.Where(w => w.E_ID == e_id).Include(inc => inc.AppUser).Select(s => new Attendee { Id = s.AppUser.Id, FullName = s.AppUser.FirstName + " " + s.AppUser.LastName }).ToList();
                eventBuilder.Image_Path = "." + eve.Img_Path;

                return View(eventBuilder);
            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Edit(EventBuilder eventBuilder)
        {
            var ReturnResult = false;

            var originalEvent = await _dbContext.Event.Where(w => w.E_ID == eventBuilder.E_ID).FirstOrDefaultAsync();

            var now = DateTime.Now;

            eventBuilder.IsPastEvent = (DateTime.Compare(originalEvent.Date.Date, now.Date) > 0) ? false
                : ((DateTime.Compare(originalEvent.Date.Date, now.Date) == 0) ?
                ((TimeSpan.Compare(originalEvent.Time.TimeOfDay, now.TimeOfDay) > 0) ? false : true) : true);

            if (!ModelState.IsValid)
            {
                ReturnResult = false;
            }
            else
            {
                if (originalEvent == null)
                {
                    ModelState.AddModelError("", "Unable to save changes. The Event was deleted by another user.");
                    ReturnResult = false;
                }

                try
                {
                    var New_Img_Path = originalEvent.Img_Path;

                    if (eventBuilder.Image != null)
                    {
                        var date = Request;
                        var files = Request.Form.Files;
                        long size = files.Sum(f => f.Length);
                        string contentRootPath = _hostingEnvironment.ContentRootPath;
                        IFormFile img = eventBuilder.Image;


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
                            var filePath = webRootPath + "./sources/eventImg/" + newFileName;
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await img.CopyToAsync(stream);
                            }
                            New_Img_Path = "./sources/eventImg/" + newFileName;
                        }
                    }


                    var entry = _dbContext.Entry(originalEvent);
                    entry.Property("RowVersion").OriginalValue = eventBuilder.RowVersion;

                    if (await TryUpdateModelAsync<Event>(originalEvent, "", x => x.Name, x => x.Description, x => x.Location, x => x.Img_Path, x => x.Date, x => x.Time))
                    {
                        try
                        {
                            using (var transaction = _dbContext.Database.BeginTransaction())
                            {

                                originalEvent.Img_Path = New_Img_Path;
                                originalEvent.Name = eventBuilder.Name;
                                originalEvent.Location = eventBuilder.Location;
                                originalEvent.Description = eventBuilder.Description;

                                if (!eventBuilder.IsPastEvent)
                                {
                                    originalEvent.Date = eventBuilder.Date;
                                    originalEvent.Time = eventBuilder.Time;
                                }

                                _dbContext.Event.Update(originalEvent);
                                await _dbContext.SaveChangesAsync();

                                if (!eventBuilder.IsPastEvent) // only update attendees and combines when it is not the past event
                                {

                                    // check if combine id need to update
                                    var event_Builder_Check = _dbContext.Event_Builder.Where(e => e.E_ID == eventBuilder.E_ID).FirstOrDefault();

                                    if (event_Builder_Check.C_ID != eventBuilder.Combine.C_ID)
                                    {
                                        event_Builder_Check.C_ID = eventBuilder.Combine.C_ID;
                                        _dbContext.Event_Builder.Update(event_Builder_Check);
                                        await _dbContext.SaveChangesAsync();
                                    }

                                    // check the new attendees received from page.

                                    var originalAttendees = _dbContext.Event_Assigned_Attendee.Where(a => a.E_ID == eventBuilder.E_ID).ToList();

                                    var newAttendeesToInsert = eventBuilder.Attendees.Where(w => !originalAttendees.Select(s => s.Id).Contains(w.Id) && w.Id != 0)
                                                                .Select(ss =>
                                                                new Event_Assigned_Attendee
                                                                {
                                                                    E_ID = eventBuilder.E_ID,
                                                                    Id = ss.Id
                                                                }).ToList();
                                    if (newAttendeesToInsert.Count > 0)
                                    {
                                        await _dbContext.Event_Assigned_Attendee.AddRangeAsync(newAttendeesToInsert);
                                        await _dbContext.SaveChangesAsync();
                                    }

                                    // check the removed previous attendees 

                                    originalAttendees.RemoveAll(x => eventBuilder.Attendees.Select(s => s.Id).Contains(x.Id));
                                    var attendeesToRemove = originalAttendees;

                                    if (attendeesToRemove.Count > 0)
                                    {
                                        _dbContext.Event_Assigned_Attendee.RemoveRange(attendeesToRemove);
                                        await _dbContext.SaveChangesAsync();
                                    }
                                }

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
                                var databaseValues = (Event)databaseEntry.ToObject();

                                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                        + "was modified by another user after you got the original value. The "
                                        + "edit operation was canceled. If you still want to edit this record, click "
                                        + "the Save button again. Otherwise click the Back to List all tests.");

                                eventBuilder.RowVersion = (byte[])databaseValues.RowVersion;

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

            if (ReturnResult)
            {
                return RedirectToAction("Events");
            }
            else
            {
                // just in case of some user change the value of iteams which are not supposed to be allowed to change on html, then return them the correct value
                if (eventBuilder.IsPastEvent)
                {
                    eventBuilder.Date = originalEvent.Date;
                    eventBuilder.Time = originalEvent.Time;
                    eventBuilder.Combine = _dbContext.Event_Builder.Where(w => w.E_ID == originalEvent.E_ID).Include(inc => inc.Combine).Select(s => s.Combine).FirstOrDefault();
                    eventBuilder.Attendees = _dbContext.Event_Assigned_Attendee.Where(w => w.E_ID == originalEvent.E_ID).Include(inc => inc.AppUser).Select(s => new Attendee { Id = s.AppUser.Id, FullName = s.AppUser.FirstName + " " + s.AppUser.LastName }).ToList();
                }

                return View(eventBuilder);
            }
        }


        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult CreateEvents()
        {
            return View();
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> CreateEvents(EventBuilder eventBuilder)
        {
            if (!ModelState.IsValid)
            {
                return View(eventBuilder);
            }
            else
            {
                try
                {
                    var Img_Path = "";

                    if (eventBuilder.Image != null)
                    {
                        var date = Request;
                        var files = Request.Form.Files;
                        long size = files.Sum(f => f.Length);

                        string contentRootPath = _hostingEnvironment.ContentRootPath;
                        IFormFile img = eventBuilder.Image;


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
                            var filePath = webRootPath + "./sources/eventImg/" + newFileName;
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await img.CopyToAsync(stream);
                            }
                            Img_Path = "./sources/eventImg/" + newFileName;
                        }
                    }
                    else if (eventBuilder.Image == null)
                    {
                        Img_Path = _configuration["EventImgNullString:Value"];
                    }
                    var eve = new Event
                    {
                        Name = eventBuilder.Name,
                        Description = eventBuilder.Description,
                        Location = eventBuilder.Location,
                        Date = eventBuilder.Date,
                        Img_Path = Img_Path,
                        Time = eventBuilder.Time
                    };

                    _dbContext.Event.Update(eve);
                    _dbContext.SaveChanges();

                    var eve_builder = new Event_Builder
                    {
                        E_ID = eve.E_ID,
                        C_ID = eventBuilder.Combine.C_ID
                    };

                    _dbContext.Event_Builder.Update(eve_builder);
                    _dbContext.SaveChanges();

                    if (eventBuilder.Attendees != null)
                    {
                        foreach (var x in eventBuilder.Attendees)
                        {
                            if (x != null && x.Id != 0)
                            {
                                var event_Assigned_Attendees = new Event_Assigned_Attendee
                                {
                                    E_ID = eve.E_ID,
                                    Id = x.Id
                                };

                                _dbContext.Event_Assigned_Attendee.Update(event_Assigned_Attendees);
                                _dbContext.SaveChanges();
                            }
                        }

                    }
                    return RedirectToAction("Events");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                                "Try again, and if the problem persists, " +
                                "see your system administrator.");
                    return View(eventBuilder);
                }
            }
        }
    }
}