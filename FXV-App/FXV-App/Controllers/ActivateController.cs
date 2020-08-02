using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using FXV.Data;
using FXV.Models;
using FXV.JwtManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV.Controllers
{
    public class ActivateController : Controller
    {
        private readonly CustomUserManager<AppUser> userManager;

        public ActivateController(CustomUserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        // GET: /<controller>/
        public async Task<IActionResult> SignUpActivate(string var1, string var2)
        {
            var user = await userManager.FindByNameAsync(var1);

            var code = WebUtility.UrlDecode(var2.Replace("+","%2B"));

            var result = await userManager.ConfirmEmailAsync(user,code);

            if (result.Succeeded)
            {
                TempData["message_header"] = "Success!";
                TempData["message_content"] = "Your account has activated. Please login with your email and password";
                return View();
            }
            else
            {
                TempData["message_header"] = "Faild!";
                TempData["message_content"] = "Invalid link, please contact with our admin";
                return View();
            }

        }
    }
}

