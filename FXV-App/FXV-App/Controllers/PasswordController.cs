using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FXV.Data;
using FXV.JwtManager;
using FXV.Models;
using FXV.PasswordHasher;
using FXV.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class PasswordController : Controller
    {
        private readonly CustomUserManager<AppUser> userManager;
        private JwtHandler jwtHandler;
        private IConfiguration configuration;
        private ApplicationDbContext applicationDbContext;
        private ApplicationDbContext dbContext;

        public PasswordController(IHttpContextAccessor accessor, CustomUserManager<AppUser> userManager, IConfiguration configuration, ApplicationDbContext applicationDbContext, ApplicationDbContext dbContext)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.jwtHandler = new JwtHandler(accessor, userManager, configuration);
            this.applicationDbContext = applicationDbContext;
            this.dbContext = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Reset(int id, string var1, string var2, string var3)
        {
            if (jwtHandler.ValidateToken(var3) && id != 0 && var1 != null && var1 != "" && var2 != null && var2 != "" && var3 != null && var3 != "")
            {
                if (dbContext.UserTokens.Where(w=>w.UserId == id && w.Name == "asp_token_pwd_reset" && w.Value == var2).Count() == 0 || dbContext.UserTokens.Where(w => w.UserId == id && w.Name == "jwt_token_pwd_reset" && w.Value == var3).Count() == 0)
                {
                    return RedirectToAction("Redirect", "Account", new { message_header = "Faild!", message_content = "Invalid link! Try to click 'Forgot password' to request a new reset password link. If it keeps happened, please contact with our admin." });
                }

                var uid = id;
                var email = var1;
                var token = var2;

                return View("SetNewPassword",new ResetPwd {UID = uid, Email = email, UserManagerToken = token});
            }
            else
            {
                return RedirectToAction("Redirect","Account",new { message_header = "Faild!", message_content = "Invalid link! Try to click 'Forgot password' to request a new reset password link. If it keeps happened, please contact with our admin." });
            }
        }

        [HttpGet]
        public IActionResult SetNewPassword(ResetPwd resetPwd)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmNewPwd(ResetPwd resetPwd)
        {
            if (!resetPwd.NewPwd.Equals(resetPwd.ConfirmNewPwd))
            {
                TempData["ErrorMsg"] = "Password not match.";
                return View("SetNewPassword", resetPwd);
            }

            var user = await userManager.FindByIdAsync(resetPwd.UID.ToString());

            if (!ModelState.IsValid)
            {
                return View("SetNewPassword", resetPwd);
            }
            else
            {
                var code = WebUtility.UrlDecode(resetPwd.UserManagerToken.Replace("+", "%2B"));

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                byte[] buffer = new byte[16];

                rng.GetBytes(buffer);

                string salt = BitConverter.ToString(buffer);

                string salt1 = salt.Substring(0, salt.Length / 2);
                string salt2 = salt.Substring(salt.Length / 2);

                var hashedPwd = new PasswordHandler().GetEncrptedPWD(resetPwd.NewPwd, salt1, salt2);

                var result = await userManager.ResetPasswordAsync(user, code, hashedPwd);

                if (result.Succeeded)
                {
                    user = applicationDbContext.Users.Find(resetPwd.UID);

                    user.PasswordHash = hashedPwd;
                    user.Salt_1 = salt1;
                    user.Salt_2 = salt2;

                    applicationDbContext.Users.Update(user);

                    await applicationDbContext.SaveChangesAsync();

                    applicationDbContext.UserTokens.RemoveRange(dbContext.UserTokens.Where(w => w.UserId == resetPwd.UID && (w.Name == "asp_token_pwd_reset" || w.Name == "jwt_token_pwd_reset"))); // remove tokens, to avoid user reset their pwd multiple times by using same link

                    await applicationDbContext.SaveChangesAsync();

                    return RedirectToAction("Redirect", "Account", new { message_header = "Success!", message_content = "Your have reset your password. Please login with your new password" });
                }
                else
                {
                    TempData["ErrorMsg"] = "There is an error, try again. If keeps happened, please contact with our admin";
                    return View("SetNewPassword", resetPwd);
                }
            }
        }
    }
}
