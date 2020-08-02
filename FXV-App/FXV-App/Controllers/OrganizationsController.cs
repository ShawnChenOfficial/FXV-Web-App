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
    public class OrganizationsController : CustomizeController
    {
        public OrganizationsController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("All_NoAthlete")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Organizations()
        {
            return View();
        }


        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        //
        //This method is used to create new organization
        //
        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Create(OrganizationBuilder organizationBuilder) 
        {
            if (!ModelState.IsValid)
            {
                return View(organizationBuilder);
            }
            else
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var Img_Path = "";
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

                                Img_Path = "./sources/orgImg/" + newFileName;
                            }
                        }
                        else if (organizationBuilder.Image == null)
                        {
                            Img_Path = _configuration["OrgImgNullString:Value"];
                        }

                        var organizations = new Organization
                        {
                            Id = organizationBuilder.Owner.Id,
                            Img_Path = Img_Path,
                            Description = organizationBuilder.Description,
                            Location = organizationBuilder.Location,
                            Name = organizationBuilder.Name
                        };

                        _dbContext.Organization.Update(organizations);
                        _dbContext.SaveChanges();

                        var organization_Relationships = new Organization_Relationship
                        {
                            Org_ID = organizations.Org_ID,
                            Id = organizations.Id,
                            Role = "Organization Manager"
                        };

                        _dbContext.Organization_Relationship.Update(organization_Relationships);
                        _dbContext.SaveChanges();

                        var user = await _userManager.FindByIdAsync(organization_Relationships.Id.ToString());

                        if (await _userManager.IsInRoleAsync(user, "Athlete"))
                        {
                            await _userManager.RemoveFromRoleAsync(user, "Athlete");
                            await _userManager.AddToRoleAsync(user, "Manager");
                            _dbContext.SaveChanges();
                        }

                        transaction.Commit();
                        return RedirectToAction("Organizations", "Organizations");

                    }
                    catch (Exception e)
                    {

                        ModelState.AddModelError("", "Unable to save changes. " +
                                           "Try again, and if the problem persists, " +
                                           "see your system administrator.");

                        return View(organizationBuilder);
                    }

                }
            }
        }

    }
}
