using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Controllers
{
    public class Layout : Controller
    {

        public IActionResult JudgeIndex()
        {
            var nemeData = HttpContext.User.Claims.FirstOrDefault(m => m.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (nemeData != null)
            {
                return RedirectToAction("Logout");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Layout4()
        {
            return View();
        }
        public IActionResult Layout3()
        {
            return View();
        }
        public IActionResult Layout2()
        {
            return View();
        }
        public IActionResult Layout1()
        {
            return View();
        }

        public IActionResult QA()
        {
            return View();
        }


        public IActionResult Heroes()
        {
            return View();
        }
        public IActionResult Features()
        {
            return View();
        }
        public IActionResult Footers()
        {
            return View();
        }
        public IActionResult Album()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");//導至登入頁
        }
    }
}
