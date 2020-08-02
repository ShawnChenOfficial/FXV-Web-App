using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FXV.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class ErrorsController : Controller
    {

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int? statusCode)
        {
            if (statusCode.HasValue)
            {
                switch (statusCode)
                {
                    case 403:
                        return View("Forbid_403");
                }
            }
            return View("DefaultErrorpage", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Forbid_403()
        {
            return View();
        }

        public IActionResult DefaultErrorPage()
        {
            return View();
        }
    }
}
