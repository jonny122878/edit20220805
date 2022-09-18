using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using edit20210325.ViewModels;

namespace edit20210325.Controllers
{
    public class ConsultNowController : Controller
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
                return RedirectToAction("SendEmail");
            }
        }
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(ConsultNowViewModel ConsultNow)
        {
            if(ModelState.IsValid)  
            {
                try
                {
                    SmtpClient SmtpServer = new SmtpClient("smtp.hibox.biz");
                    SmtpServer.Port = 25;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(
                        "service@topefficiencywork.tw", "Jonny1070607!@#$%");

                    SmtpServer.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("service@topefficiencywork.tw", "早點下班–網頁客戶諮詢");
                    mail.To.Add("topefficiencywork@gmail.com");
                    mail.Subject = DateTime.Now.ToString() + "客戶來信諮詢";

                    mail.Body = String.Format("客戶信箱：{0}<br><br> 諮詢內容：<br>{1}<br><br>{2}",
                        ConsultNow.Email, ConsultNow.Consultation, DateTime.Now.ToString());

                    mail.IsBodyHtml = true;

                    SmtpServer.Send(mail);
                    TempData["ConsultationStatus"] = "已收到您的寶貴意見，我們將儘速和你聯絡";
                    //ConsultNow.ConsultationStatus = "已收到您的寶貴意見，我們將儘速和你聯絡";

                    //return View();


                    return Redirect("~/ConsultNow/SendEmail");
                }
                catch
                {
                    TempData["ConsultationStatus"] = "寄信失敗，請透過Line與我們聯絡  @topefficiencywork";
                    //ConsultNow.ConsultationStatus = "寄信失敗，請透過Line與我們聯絡  @topefficiencywork";
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");//導至登入頁
        }
    }
}
