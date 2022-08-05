using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using edit20210325.Function;
using edit20210325.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace edit20210325.Controllers
{
    [Area(ProjectSet.AdminName)]
    public class LoginController : Controller
    {
        /// <summary>
        /// 讀取組態用
        /// </summary>
        private readonly IConfiguration config;
        public LoginController(IConfiguration config)
        {
            this.config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 表單post提交，準備登入
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(string inputUsername, string inputPassword, string ReturnUrl)
        {//未登入者想進入必須登入的頁面，他會被導頁至/Home/Login，網址後面會加上QueryString:ReturnUrl(原始要求網址)
            
            var db = new CASE20210405Context();
            List<ManagerModel> lsModel = db.ManagerModels.ToList();
            ManagerModel it = lsModel.Where(c => c.ManagerUser == inputUsername).FirstOrDefault();
            //從自己的DB檢查帳&密，輸入是否正確
            if (it == null || (inputUsername ==it.ManagerUser && inputPassword == it.ManagerPassword) == false)
            {
                //帳&密不正確
                ViewBag.errMsg = "帳號或密碼輸入錯誤!!";
                return View();//流程不往下執行
            }

            //帳密都輸入正確，ASP.net Core要多寫三行程式碼 
            //Claim[] claims = new[] { new Claim("Account", inputUsername) }; //取名Account，在登入後的頁面，讀取登入者的帳號會用得到，自己先記在大腦
            Claim[] claims = new[] { new Claim(ClaimTypes.NameIdentifier, it.ManagerName) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);//Scheme必填
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            //執行登入，相當於以前的FormsAuthentication.SetAuthCookie()
            //從組態讀取登入逾時設定
            double loginExpireMinute = this.config.GetValue<double>("LoginExpireMinute");
            await HttpContext.SignInAsync(principal,
                new AuthenticationProperties()
                {
                    IsPersistent = false, //IsPersistent = false，瀏覽器關閉即刻登出
                                          //用戶頁面停留太久，逾期時間，在此設定的話會覆蓋Startup.cs裡的逾期設定
                    /* ExpiresUtc = DateTime.Now.AddMinutes(loginExpireMinute) */
                });
            //加上 Url.IsLocalUrl 防止Open Redirect漏洞
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);//導到原始要求網址
            }
            else
            {
                return RedirectToAction("Index", "Admin");//到登入後的第一頁，自行決定
            }

        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        //登出 Action 記得別加上[Authorize]，不管用戶是否登入，都可以執行Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Login");//導至登入頁
        }
    }
}
