using edit20210325.Function;
using edit20210325.Models;
using edit20210325.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UnilityGoogle;

namespace edit20210325.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 讀取組態用
        /// </summary>
        private readonly IConfiguration config;
        private readonly CASE20210405Context _dbContext;
        public HomeController(IConfiguration config)
        {
            this.config = config;
            this._dbContext = new CASE20210405Context();
        }

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

            ProjectSet.GoogleUrl = "https://" + HttpContext.Request.Host + "/home/callback";

            //var test = DESFunction.MD5Encrypt("R123043339", ProjectSet.PASSWORDKEY);
            //var name = DESFunction.MD5Decrypt(test, ProjectSet.PASSWORDKEY);
            
            return View();
        }

        public IActionResult LoginByWinForm([FromQuery] LoginViewModel loginViewModel)
        {

            var isLogin = this._dbContext.MemberModels.Any(r => 
            r.MemberGmail == loginViewModel.Email &&
            r.MemberPassword == loginViewModel.Password
            );

            var validLoginViewModel = new ValidLoginViewModel { Succeeded = false };
            if (isLogin)
            {
                var jsonLogErr = JsonConvert.SerializeObject(validLoginViewModel);
                return new JsonResult(jsonLogErr);
            }
            //
            //where user id and time > 0
            
            //where user id and createtime add period comaprer > current datetime now
            


            //union times and peroid
            
            //except authority false          


            //write log

            var json = JsonConvert.SerializeObject(validLoginViewModel);
            return new JsonResult(json);
        }


        public IActionResult TestAPI()
        {

            var test = new TestAPIModel {ID = "ID",Name = "NAME" };
            var json = JsonConvert.SerializeObject(test);
            return new JsonResult(json);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.User = null;

            return RedirectToAction("Index");//導至登入頁
        }

        public async Task<IActionResult> Callback(string code)
        {

            var url = "https://" + HttpContext.Request.Host + HttpContext.Request.Path;
            //get token from code
            var token = UtilityGoogles.GetTokenFromCode(code,
               ProjectSet.clientId,
               ProjectSet.clientPassword,
                url);
            var UserInfoResult = UtilityGoogles.GetUserInfo(token.access_token);

            MarkGmailData gmailData = new MarkGmailData();
            gmailData.Name = UserInfoResult.name;
            gmailData.Gmail = UserInfoResult.email;
            gmailData.Info = UserInfoResult.picture;

            var db = new CASE20210405Context();
            var principal = new LoginSave(gmailData,"false").getClaimsPrincipal(); ;
            double loginExpireMinute = this.config.GetValue<double>("LoginExpireMinute");
            await HttpContext.SignInAsync(principal,
                new AuthenticationProperties()
                {
                    IsPersistent = false, //IsPersistent = false，瀏覽器關閉即刻登出
                                          //用戶頁面停留太久，逾期時間，在此設定的話會覆蓋Startup.cs裡的逾期設定
                    /* ExpiresUtc = DateTime.Now.AddMinutes(loginExpireMinute) */
                });
            List<MemberModel> lsModel = db.MemberModels.ToList();
            List<MemberModel> ls = lsModel.Where(c => c.MemberGmail == gmailData.Gmail).ToList();
            if (ls.Count == 0) return RedirectToAction("MemberRegister", "MemberInfo");
            else
            {
                var principalOn = new LoginSave(gmailData, "true").getClaimsPrincipal();
                await HttpContext.SignInAsync(principalOn,
                    new AuthenticationProperties()
                    {
                        IsPersistent = false, //IsPersistent = false，瀏覽器關閉即刻登出
                    });
                return RedirectToAction("SaveCash", "MemberInfo");//到登入後的第一頁，自行決定
            }
            
        }
        
        [HttpPost]
        public string SaleOrderHashKey(string Gid)
        {
            var db = new CASE20210405Context();
            var hashKeys = new HashKeys();
            hashKeys.HashKey = DESFunction.GetRandomStringByHashKey(8);
            var SaleOrderModel = db.SalesOrderModels.Find( Guid.Parse(Gid));
            if (SaleOrderModel == null)
            {
                hashKeys.EncryptionCode = "";
                hashKeys.Messages = "搜尋錯誤";              
            }
            else
            {
                var codeData = SaleOrderModel.Id.ToString() ;
                if (SaleOrderModel.ProductCharge.Equals("期間"))
                {
                     codeData += "," +
                                SaleOrderModel.ProductChangeDate.ToString("yyyy/MM/dd HH:mm:ss") + "," +
                                SaleOrderModel.ProductDays.ToString();

                }
                else
                {
                    var totalCountsLs = db.ConsumerOrderLogModels.Where(c => c.MemberId == SaleOrderModel.MemberId &&
                                         c.SalesOrderId == SaleOrderModel.Id.ToString()).ToList();
                    int totalCounts = 0;
                    if (totalCountsLs.Count != 0)
                    {
                        totalCounts = totalCountsLs.Select(c => c.MemberCashInCounts).Aggregate((r, c) => r + c);
                    }
                    codeData +=","+totalCounts+"," + SaleOrderModel.ProductCounts;
                }
                hashKeys.EncryptionCode = DESFunction.DESEncrypt(codeData, ProjectSet.PASSWORDKEY, hashKeys.HashKey);
                hashKeys.Messages = "OK";
            }           
            string JsonDataout = JsonConvert.SerializeObject(hashKeys);
            return JsonDataout;
        }

        [HttpPost]
        public string SaleOrderWirteCounts(string Code,string Hashkey)
        {
            string CodeData = DESFunction.DESDecrypt(Code, ProjectSet.PASSWORDKEY, Hashkey);
            try
            {
                var CodeLs = CodeData.Split(',').ToList();
                var Gid = CodeLs[0];
                var counts = Convert.ToInt32(CodeLs[1]);
                var db = new CASE20210405Context();
                var SaleOrderModel = db.SalesOrderModels.Find(Guid.Parse(Gid));
                if (SaleOrderModel == null) return "NG";
                if (SaleOrderModel.ProductCharge.Equals("期間")) return "NG";
                SaleOrderModel.ProductCounts = counts;
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "NG";
            }        
        }

        [HttpPost]
        public string ReadControlHashKey(string Gid)
        {
            var db = new CASE20210405Context();
            var hashKeys = new HashKeys();
            hashKeys.HashKey = DESFunction.GetRandomStringByHashKey(8);
            var FileLoadModel = db.FileLoadInfoModels.Find(Guid.Parse(Gid));
            var MemberModel = db.MemberModels.Find(Guid.Parse(FileLoadModel.MmberId));
            if (FileLoadModel == null || MemberModel == null)
            {
                hashKeys.EncryptionCode = "";
                hashKeys.Messages = "搜尋錯誤";
            }
            else
            {
                var codeData = FileLoadModel.Id.ToString();
                codeData += "," + FileLoadModel.FileMac;
                var idenData = DESFunction.DESDecrypt(MemberModel.MemberIden, ProjectSet.PASSWORDKEY);
                if (idenData.Length == 8) 
                    codeData += ",C";
                else 
                    codeData += ",P";
                hashKeys.EncryptionCode = DESFunction.DESEncrypt(codeData, ProjectSet.PASSWORDKEY, hashKeys.HashKey);
                hashKeys.Messages = "OK";
            }
            string JsonDataout = JsonConvert.SerializeObject(hashKeys);
            return JsonDataout;
        }

        [HttpPost]
        public string WriteControlHashKey(string Code, string Hashkey)
        {
            string CodeData = DESFunction.DESDecrypt(Code, ProjectSet.PASSWORDKEY, Hashkey);
            try
            {
                var CodeLs = CodeData.Split(',').ToList();
                var Gid = CodeLs[0];
                var mac = CodeLs[1];
                var db = new CASE20210405Context();
                var FileLoadInfoModel = db.FileLoadInfoModels.Find(Guid.Parse(Gid));
                if (FileLoadInfoModel == null) return "NG";
                FileLoadInfoModel.FileMac = mac;
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "NG";
            }
        }

    }
}
