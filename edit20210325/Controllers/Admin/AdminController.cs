using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using edit20210325.Models;
using System.Security.Claims;
using edit20210325.Function;

namespace edit20210325.Controllers
{
    [Area(ProjectSet.AdminName)]
    public class AdminController : Controller
    {
      
        public IActionResult Index()
        {
            if (HttpContext.User.Claims.Count() == 0 ) return RedirectToAction("Index","Login");
            string Account = HttpContext.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            return View();
        }

    }
}
