using edit20210325.Models;
using edit20210325.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;

namespace edit20210325.Controllers
{
    public class DownloadController : Controller
    {
        private readonly CASE20210405Context _dbContext;
        public DownloadController()
        {
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
            ViewData["Debug"] = @"C:\Program_Tsai\edit20210325\edit20210325\Programs\Debug.zip";
            return View();
        }
        [HttpPost]
        public IActionResult DownloadFile(string filename)
        {
            //string file = @"C:\Program_Tsai\edit20220409網頁程式\edit20210512\edit20210325\edit20210325\wwwroot\test1.txt";
            //string contentType = "";
            //var provider = new FileExtensionContentTypeProvider();
            //provider.TryGetContentType(file, out contentType);
            //byte[] fileBytes = System.IO.File.ReadAllBytes(file);
            //return File(fileBytes, contentType, "result.txt");

            var directory = Directory.GetCurrentDirectory() + @"\Programs";
            //var filename = "Debug.zip";

            string file = Path.Combine(directory, filename);
            string contentType = "";
            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(file, out contentType);
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);
            return File(fileBytes, contentType, "Setup1.msi");
        }

        public IActionResult GetFile()
        {
            //string file = @"C:\Program_Tsai\edit20220409網頁程式\edit20210512\edit20210325\edit20210325\wwwroot\test1.txt";
            //string contentType = "";
            //var provider = new FileExtensionContentTypeProvider();
            //provider.TryGetContentType(file, out contentType);
            //byte[] fileBytes = System.IO.File.ReadAllBytes(file);
            //return File(fileBytes, contentType, "result.txt");

            var directory = Directory.GetCurrentDirectory() + @"\Programs";
            var filename = "Debug.zip";

            string file = Path.Combine(directory, filename);
            string contentType = "";
            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(file, out contentType);
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);
            return File(fileBytes, contentType, "Debug.zip");
        }
        public IActionResult UpdateVersion([FromQuery] UpdateVersionViewModel updateVersionViewModel)
        {

            #region 清除要更新產品資料夾和zip
            var pathSendZip = Directory.GetCurrentDirectory() + @"\Programs";
            var filenameSendZip = "SendZip.zip";
            var fileSendZip = Path.Combine(pathSendZip, filenameSendZip);
            System.IO.File.Delete(fileSendZip);
            var directory = Directory.GetCurrentDirectory() + @"\Programs\SendZip";
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);
            #endregion
            #region 比對發送本地端將要更新zip檔搬移
            var sourcePath = Directory.GetCurrentDirectory() + @"\Programs\" + updateVersionViewModel.User;
            var sourceFiles = Directory.GetFiles(sourcePath).Select(r =>
            {
                var key = System.IO.Path.GetFileNameWithoutExtension(r);
                return new KeyValuePair<string, string>(key, r);
            }).ToList();
            Console.WriteLine("");
            sourceFiles.Join(updateVersionViewModel.Versions,
                inner => inner.Key,
                outer => outer,
                (inner, outer) => inner.Value).ToList()
                .ForEach(item =>
                {
                    var destFilename = Path.GetFileName(item);
                    var destFile = Path.Combine(directory, destFilename);
                    System.IO.File.Copy(item, destFile, true);
                });
            #endregion
            Console.WriteLine("");
            //batch壓縮
            ZipFile.CreateFromDirectory(directory, fileSendZip);
            Console.Write("");
            var filename = "SendZip.zip";
            string file = Path.Combine(pathSendZip, filename);
            string contentType = "";
            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(file, out contentType);
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);
            return File(fileBytes, contentType, filename);
        }

        public IActionResult LoginByWinForm([FromQuery] LoginViewModel loginViewModel)
        {

            var names = this._dbContext.MemberModels.FirstOrDefault(r =>
            r.MemberGmail == loginViewModel.Email &&
            r.MemberPassword == loginViewModel.Password
            );

            var validLoginViewModel = new ValidLoginViewModel { Succeeded = false };
            if (names == null)
            {
                var jsonLogErr = JsonConvert.SerializeObject(validLoginViewModel);
                return new JsonResult(jsonLogErr);
            }

            //過期情境
            //5/1 30天,5/15 30天
            //8/18 30天
            //思路 最近一張單過期
            //全部加總過期
            //才算是過期

            var memberCashIns = this._dbContext.MemberCashInModels.AsEnumerable().ToList();
            var saleOrders = this._dbContext.SalesOrderModels.AsEnumerable().ToList();
            Console.Write("");
            //KeyValuePair<string, bool>單號,是否過期

            //最接近現在的單再加天數
            var lastOvers = this._dbContext.SalesOrderModels.AsEnumerable().GroupBy(r => r.SalesOrderOrderID)
                .Select(r =>
                {
                    var item = r.Max(r => r.SalesOrderItem);
                    var createTime = r.First(c => c.SalesOrderItem == item).ProductCreateDate;
                    var days = r.First(c => c.SalesOrderItem == item).ProductDays;
                    createTime = createTime.AddDays(days);
                    var cmp = createTime.CompareTo(DateTime.Now);
                    var isOver = (cmp == 1);
                    return new KeyValuePair<string, bool>(r.Key, isOver);
                }).ToList();
            Console.WriteLine("");

            var sumOvers = this._dbContext.SalesOrderModels.AsEnumerable().GroupBy(r => r.SalesOrderOrderID)
                .Select(r =>
                {
                    var minTime = r.Min(c => c.ProductCreateDate);
                    var sumDay = r.Sum(c => c.ProductDays);
                    var overTime = minTime.AddDays(sumDay);
                    var cmp = overTime.CompareTo(DateTime.Now);
                    var isOver = (cmp == 1);
                    return new KeyValuePair<string, bool>(r.Key, isOver);
                }).ToList();

            Console.WriteLine("");
            //產品清單
            //Map出剩餘天數
            //Group產品數量加總
            //Join補足其他資訊
            //KeyValuePair<string, Tuple<string,bool>>單號,是否過期
            var queryPeroids = from o in this._dbContext.MemberCashInModels.AsEnumerable().Where(r => r.MemberCashInMemberGmail == loginViewModel.Email)
                               join l in lastOvers on o.MemberCashInOrderID equals l.Key
                               join s in sumOvers on o.MemberCashInOrderID equals s.Key
                               select new
                               {
                                   OrderID = o.MemberCashInOrderID,
                                   Name = o.MemberCashInName,
                                   Version = o.MemberCashInVersion,
                                   IsOver = (l.Value == false && s.Value == false) ? false : true
                               };
            var peroids = queryPeroids.AsEnumerable().Select(r =>
            {
                return new KeyValuePair<string, Tuple<string, string, bool>>(r.OrderID, new Tuple<string, string, bool>(r.Name, r.Version, r.IsOver));
            }).ToList();



            Console.WriteLine("");

            validLoginViewModel.Succeeded = true;
            validLoginViewModel.Name = names.MemberName;
            validLoginViewModel.DisplayAuthentications = peroids;



            var json = JsonConvert.SerializeObject(validLoginViewModel);
            return new JsonResult(json);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");//導至登入頁
        }
    }
}
