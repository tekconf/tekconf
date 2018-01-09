using System;
using Microsoft.AspNetCore.Mvc;

namespace tekconf.web.Controllers
{

    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
