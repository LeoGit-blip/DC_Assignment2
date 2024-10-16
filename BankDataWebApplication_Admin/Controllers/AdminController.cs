﻿using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_Admin.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/Home/Index.cshtml");
        }

        [HttpGet("login")]
        public IActionResult GetLoginPage()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("~/Views/AdminLogin/AuthenticatedAdminLoginPage.cshtml");
                }
            }
            return PartialView("~/Views/AdminLogin/AdminLoginPage.cshtml");
        }

        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("~/Views/AdminLogin/ErrorInAdminLoginPage.cshtml");
        }
    }
}
