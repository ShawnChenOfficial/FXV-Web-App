using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FXV.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using FXV.JwtManager;
using Microsoft.AspNetCore.Identity;
using FXV.Models;
using FXV.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using FXV.PasswordHasher;
using System.Security.Cryptography;
using Newtonsoft.Json;
using FXV_App.CustomizeControllers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class SettingsController : CustomizeController
    {
        public SettingsController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }


        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePwdParams changePwdParams, byte[] rowVersion)
        {
            if (!ModelState.IsValid)
            {
                changePwdParams = new ChangePwdParams();
                return View("Settings",changePwdParams);
            }
            else
            {
                var user = await _dbContext.Users.FindAsync(_claim.Uid);

                var hashedInputCurrentPwd = new PasswordHandler().GetEncrptedPWD(changePwdParams.CurrentPassword, user.Salt_1, user.Salt_2);

                if (hashedInputCurrentPwd == user.PasswordHash)
                {
                    if (!changePwdParams.NewPassword.Equals(changePwdParams.ConfirmNewPassword))
                    {
                        ModelState.AddModelError("", "New passwords not match.");
                        changePwdParams = new ChangePwdParams();
                        return View("Settings", changePwdParams);
                    }
                    else if (changePwdParams.CurrentPassword == changePwdParams.NewPassword)
                    {
                        ModelState.AddModelError("", "You Can't set your new password as same as your current password");
                        changePwdParams = new ChangePwdParams();
                        return View("Settings", changePwdParams);
                    }
                    else
                    {
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                        byte[] buffer = new byte[16];

                        rng.GetBytes(buffer);

                        string salt = BitConverter.ToString(buffer);

                        string salt1 = salt.Substring(0, salt.Length / 2);
                        string salt2 = salt.Substring(salt.Length / 2);

                        var hashedPwd = new PasswordHandler().GetEncrptedPWD(changePwdParams.NewPassword, salt1, salt2);

                        user.PasswordHash = hashedPwd;
                        user.Salt_1 = salt1;
                        user.Salt_2 = salt2;

                        var originalUser = await _dbContext.Users.Where(w=>w.Id == _claim.Uid).FirstOrDefaultAsync();

                        var entry = _dbContext.Entry(originalUser);

                        entry.Property("RowVersion").OriginalValue = entry.Entity.RowVersion;

                        if (await TryUpdateModelAsync<AppUser>(originalUser, "", e => e.PasswordHash, e => e.Salt_1, e => e.Salt_2))
                        {
                            try
                            {
                                _dbContext.Users.Update(user);
                                await _dbContext.SaveChangesAsync();
                                return View("Settings");
                            }
                            catch (DbUpdateConcurrencyException ex)
                            {
                                //Log the error (uncomment ex variable name and write a log.)
                                ModelState.AddModelError("", "Unable to save changes. " +
                                     "Try again, and if the problem persists, " +
                                     "see your system administrator.");
                                changePwdParams = new ChangePwdParams();
                                return View("Settings", changePwdParams);
                            }
                        }
                        ModelState.AddModelError("", "Unable to save changes. " +
                                    "Try again, and if the problem persists, " +
                                    "see your system administrator.");
                        changePwdParams = new ChangePwdParams();
                        return View("Settings", changePwdParams);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The current password you entered, is not correct.");
                    changePwdParams = new ChangePwdParams();
                    return View("Settings", changePwdParams);
                }
            }

        }

    }
}
