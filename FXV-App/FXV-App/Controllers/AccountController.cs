using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FXV.Data;
using FXV.JwtManager;
using FXV.Models;
using FXV.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV.Controllers
{
    public class AccountController : Controller
    {
        private readonly CustomUserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IHttpContextAccessor accessor;
        private JwtHandler jwtHandler;

        public AccountController(IHttpContextAccessor accessor, ApplicationDbContext applicationDbContext, CustomUserManager<AppUser> userManager, IConfiguration configuration)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
            this.configuration = configuration;
            this.accessor = accessor;
            this.jwtHandler = new JwtHandler(accessor, userManager,configuration);
        }


        [HttpGet]
        [HttpPost]
        public IActionResult Index()
        {
            var access_token = Request.Cookies["access_token"];

            if (access_token != null && access_token != "")
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Response.Cookies.Delete("access_token");
                }
            }

            TempData["ErrorMsg"] = null;

            return View();
        }

        [HttpGet]
        public IActionResult ResetPwd()
        {
            return View();
        }
        //SendResetLink not support multi-account users at moment.
        [HttpPost]
        public async Task<IActionResult> SendResetLink(ResetPwdAccount resetPwd)
        {
            if (ModelState.IsValid)
            {
                var list_user = applicationDbContext.Users.Where(w => w.UserName == resetPwd.Email).ToList();

                if (list_user.Count <= 0)
                {
                    TempData["ErrorMsg"] = "No email matches your input";
                    return View("ResetPwd", resetPwd);
                }
                else if (list_user.Count > 1)
                {
                    TempData["ErrorMsg"] = "Sorry, we cannot send a password reset link with multi-account email, please contact our admin.";
                    return View("ResetPwd", resetPwd);
                }
                else if (applicationDbContext.UserTokens.Where(w=>w.UserId == list_user.FirstOrDefault().Id).Count() == 0)
                {
                    TempData["ErrorMsg"] = "You have requested a password reset link, please go to your email and follow the instruction.";
                    return View("ResetPwd", resetPwd);
                }
                else
                {
                    var user = list_user[0];

                    // Send an email with this link
                    string code = await userManager.GeneratePasswordResetTokenAsync(user);

                    // added HTML encoding
                    string codeWebVersion = WebUtility.UrlEncode(code);

                    //generate a jwt token for first validation
                    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:privatekey"]));

                    int expMins = Convert.ToInt32(configuration["jwt:exp"]);

                    string jwtToken = await jwtHandler.GenerateToken(signInKey, user);

                    string token ="id=" + user.Id + "&var1=" + user.UserName + "&var2=" + codeWebVersion +"&var3=" + jwtToken;

                    var MailingService = new MailingLib.MailingService
                                            (configuration,
                                             user.Email,
                                             token);

                    var activateEmailSent = MailingService.Sending(mailingType:MailingLib.MailingService.MailingType.PwdReset);

                    if (await activateEmailSent)
                    {
                        applicationDbContext.UserTokens.Add(new IdentityUserToken<int> { UserId = user.Id, LoginProvider = "FXV", Name = "asp_token_pwd_reset", Value = WebUtility.UrlDecode(codeWebVersion.Replace("+", "%2B"))});

                        applicationDbContext.UserTokens.Add(new IdentityUserToken<int> { UserId = user.Id, LoginProvider = "FXV", Name = "jwt_token_pwd_reset", Value = jwtToken });

                        await applicationDbContext.SaveChangesAsync();

                        return RedirectToAction("Redirect", new { message_header = "Success!", message_content = "We have sent you a password reset link to you email, please check." });
                    }
                    else
                    {

                        TempData["ErrorMsg"] = "Error, failed to send you the password reset link, please try again. If keeps happened, please contact our admin.";
                        return View("ResetPwd", resetPwd);
                    }
                }
            }
            else
            {
                return View("ResetPwd",resetPwd);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckExistence([FromForm] Account model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            else
            {
                List<string> users = new List<string>();
                foreach (AppUser x in userManager.Users)
                {
                    if (x.Email == model.UserAccount)
                    {
                        users.Add(x.Email);
                    }
                }

                if (users.Count > 0)
                {

                    TempData["data"] = users;

                    if (users.Count == 1)
                    {
                        var user = await userManager.FindByEmailAsync(users[0]);
                        if (!await userManager.IsEmailConfirmedAsync(user))
                        {
                            TempData["ErrorMsg"] = "Please activate your account.";
                            return View("Index", model);
                        }
                        return RedirectToAction("EnterPassword");
                    }
                    else if (users.Count > 1)
                    {
                        return RedirectToAction("SelectAccount");
                    }
                }
                TempData["ErrorMsg"] = "No account found with this email address";
                return View("Index", model);
            }
        }
        [HttpGet]
        public IActionResult EnterPassword()
        {
            if (TempData["data"] == null)
            {
                var access_token = Request.Cookies["access_token"];

                if (access_token != null && access_token != "")
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Response.Cookies.Delete("access_token");
                        return View("Index");
                    }
                }
                else
                {
                    return View("Index");
                }
            }

            string[] temData = (string[])TempData["data"];
            List<LoginInfo> info = new List<LoginInfo>();

            foreach (AppUser x in userManager.Users)
            {
                if (x.Email == temData[0])
                {
                    LoginInfo tem = new LoginInfo();
                    tem.Account = x.Email;
                    tem.FirstName = x.FirstName;
                    tem.Id = x.Id;
                    info.Add(tem);
                }
            }

            ViewBag.UserInfo = info;
            return View();
        }
        [HttpGet]
        public IActionResult SelectAccount()
        {
            if (TempData["data"] == null)
            {
                return View("Index");
            }

            string[] temData = (string[])TempData["data"];
            List<LoginInfo> info = new List<LoginInfo>();

            foreach (AppUser x in userManager.Users)
            {
                if (x.Email == temData[0])
                {
                    LoginInfo tem = new LoginInfo();
                    tem.Account = x.Email;
                    tem.FirstName = x.FirstName;
                    tem.Id = x.Id;
                    info.Add(tem);
                }
            }
            ViewBag.UserInfo = info;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginInfo model)
        {
            if (!ModelState.IsValid || model.Id.ToString() == null || model.Id.ToString() == "")
            {
                model.Password = "";

                List<string> users = new List<string>();
                foreach (AppUser x in userManager.Users)
                {
                    if (x.Email == model.Account)
                    {
                        users.Add(x.Email);
                    }
                }

                TempData["data"] = users;
                TempData["ErrorMsg"] = "Invalid password";

                if (users.Count > 1)
                {
                    return RedirectToAction("SelectAccount");
                }

                return RedirectToAction("EnterPassword");
            }

            var user = await userManager.FindByIdAsync(model.Id.ToString());

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:privatekey"]));

                int expMins = Convert.ToInt32(configuration["jwt:exp"]);

                var access_token = await jwtHandler.GenerateToken(signInKey, expMins, user);

                Response.Cookies.Append("access_token", access_token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<string> users = new List<string>();
                foreach (AppUser x in userManager.Users)
                {
                    if (x.Email == model.Account)
                    {
                        users.Add(x.Email);
                    }
                }

                TempData["data"] = users;
                TempData["ErrorMsg"] = "Invalid password";

                if (users.Count == 1)
                {
                    return RedirectToAction("EnterPassword");
                }
                else
                {
                    return RedirectToAction("SelectAccount");
                }
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Nationalities = new SelectList(applicationDbContext.Nationality.Where(w => w.Nationality_ID > 1).OrderBy(o => o.Name), "Nationality_ID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] SignUp model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (model.Password != model.Re_Password)
                {
                    return View(model);
                }
                var user = await userManager.FindByNameAsync(model.Email);

                if (user != null)
                {
                    TempData["Error"] = "This email has registered, please change to another email address.";
                    return View(model);
                }

                if (user == null && model.Password == model.Re_Password)
                {
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                    byte[] buffer = new byte[16];

                    rng.GetBytes(buffer);

                    string salt = BitConverter.ToString(buffer);

                    string salt1 = salt.Substring(0, salt.Length / 2);
                    string salt2 = salt.Substring(salt.Length / 2);

                    user = new AppUser
                    {
                        UserName = model.Email,
                        Nationality_ID = model.Nationality_ID,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        City = model.City,
                        Gender = model.Gender,
                        DOB = model.DOB,
                        Salt_1 = salt1,
                        Salt_2 = salt2,
                        PasswordHash = new PasswordHasher.PasswordHandler().GetEncrptedPWD(model.Password, salt1, salt2),
                        SecurityStamp = "",
                        ConcurrencyStamp = "",
                        P_ID = 1,
                        PhoneNumber = model.PhoneNumber,
                        Profile_Img_Path = configuration["UserProfileIconNullString:Value"]

                    };

                    var result = await userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Athlete");
                    }

                    // Send an email with this link
                    string code = await userManager.GenerateEmailConfirmationTokenAsync(await userManager.FindByNameAsync(user.UserName));

                    // added HTML encoding
                    string codeWebVersion = WebUtility.UrlEncode(code);

                    string token = "var1=" + user.UserName + "&var2=" + codeWebVersion;

                    var activateEmailSent = new MailingLib.MailingService
                                            (configuration,
                                             model.Email,
                                             token)
                                             .Sending(mailingType: MailingLib.MailingService.MailingType.Activate);

                    return RedirectToAction("Index", "Account");
                }
            }

            return View(model);
        }

        public IActionResult Redirect(string message_header, string message_content)
        {
            var header = message_header;
            var content = message_content;
            TempData["message_header"] = header;
            TempData["message_content"] = content;

            return View();
        }
    }
}
