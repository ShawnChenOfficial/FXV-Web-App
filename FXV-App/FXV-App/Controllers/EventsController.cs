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
        public IActionResult Index()//loading the recommanded events which are decided to show by Admin
        {
            //waiting for decision of slider area

            return View();
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Create(EventBuilder eventBuilder)
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
                            var filePath = webRootPath + "/sources/eventImg/" + newFileName;
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await img.CopyToAsync(stream);
                            }
                            Img_Path = "/sources/eventImg/" + newFileName;
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