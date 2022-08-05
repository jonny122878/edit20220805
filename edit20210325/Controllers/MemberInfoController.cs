using edit20210325.Function;
using edit20210325.Models;
using edit20210325.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace edit20210325.Controllers
{
    public class MemberInfoController : Controller
    {

        /// <summary>
        /// 讀取組態用
        /// </summary>
        private readonly IConfiguration config;

        public MemberInfoController(IConfiguration config)
        {
            this.config = config;
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
            //var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            //var db = new CASE20210405Context();
            //var modelcount = db.MemberModels.Count(c => c.MemberGmail == getGmail);
            //if (modelcount == 0) return RedirectToAction("index", "home");
            //var model = db.MemberModels.Where(c => c.MemberGmail == getGmail).FirstOrDefault();
            //return View(model);
            return RedirectToAction("SaveCash", "MemberInfo");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");//導至登入頁
        }

        [HttpPost]
        public string CheckIden(string item)
        {
            if (item.Equals("00000000")) return "輸入錯誤，請重新輸入!";
            var db = new CASE20210405Context();
            var memberList = db.MemberModels.ToList();
            var chenkOut = memberList.Where(c => c.MemberIden == item).Count() > 0 ? "身分證使用中!請重新輸入" : "OK";
            return chenkOut;
        }
        [HttpPost]
        public string CheckEditIden(string item,Guid MemberId)
        {
            if (item.Equals("00000000")) return "輸入錯誤，請重新輸入!";
            var db = new CASE20210405Context();
            var memberList = db.MemberModels.ToList();
            var chenkOut = memberList.Where(c => (c.MemberIden == item) && (c.Id != MemberId)).Count() > 0 ? "身分證使用中!請重新輸入" : "OK";
            return chenkOut;
        }

        [HttpGet]
        public ActionResult MemberRegister()
        {
            var getData = HttpContext.User;
            MarkGmailData gmailData = new MarkGmailData();
            gmailData.Name = getData.Claims.ToList()[0].Value;
            gmailData.Info = getData.Claims.ToList()[1].Value;
            gmailData.Gmail = getData.Claims.ToList()[2].Value;
            ViewBag.GmailData = gmailData;
            return View();
        }
        [HttpPost]
        public async Task<RedirectToActionResult> Create(MemberModel it)
        {
            if (string.IsNullOrEmpty(it.MemberName))
                return RedirectToAction("Index");
            var db = new CASE20210405Context();
            it.MemberCreateDate = DateTime.Now.Date;
            it.MemberChangeDate = DateTime.Now.Date;
            it.MemberIden = DESFunction.DESEncrypt(it.MemberIden, ProjectSet.PASSWORDKEY);
            it.MemberPhone = DESFunction.DESEncrypt(it.MemberPhone, ProjectSet.PASSWORDKEY);
            db.MemberModels.Add(it);

            var memberSaveIt = new MemberSaveModel()
            {
                MemberSaveCash = 0,
                MemberSaveRemarks = "註冊完成",
                MemberSaveId = it.Id.ToString(),
                MemberSaveCreateDate = DateTime.Now.Date,
                MemberSaveChangeDate = DateTime.Now.Date
            };
            db.MemberSaveModels.Add(memberSaveIt);
            db.SaveChanges();

            var getData = HttpContext.User;
            MarkGmailData gmailData = new MarkGmailData();
            gmailData.Name = getData.Claims.ToList()[0].Value;
            gmailData.Info = getData.Claims.ToList()[1].Value;
            gmailData.Gmail = getData.Claims.ToList()[2].Value;

            var principalOn = new LoginSave(gmailData, "true").getClaimsPrincipal();
            await HttpContext.SignInAsync(principalOn,
                new AuthenticationProperties()
                {
                    IsPersistent = false, //IsPersistent = false，瀏覽器關閉即刻登出
                });

            return RedirectToAction("SaveCash", "MemberInfo");//到登入後的第一頁，自行決定
        }

        /// <summary>
        /// 計算目前金幣數
        /// </summary>
        /// <param name="getGmail"></param>
        /// <param name="memberOut"></param>
        /// <returns></returns>
        public int CurrentCash(string getGmail, out MemberModel memberOut)
        {
            var db = new CASE20210405Context();

            MemberModel member = db.MemberModels.Where(c => c.MemberGmail == getGmail).ToList().FirstOrDefault();

            var MemberSaveToltalLs = db.MemberSaveModels.ToList().Where(c => c.MemberSaveId == (member.Id).ToString()).ToList();
            int MemberSaveToltal = 0;
            if (MemberSaveToltalLs.Count != 0)
            {
                MemberSaveToltal = MemberSaveToltalLs.Select(c => c.MemberSaveCash).Aggregate((r, s) => r + s);
            }

            var cashInConsumerToltalLs = db.ConsumerOrderLogModels.Where(c => c.MemberId == member.Id.ToString()).ToList();
            int cashInConsumerToltal = 0;
            if (cashInConsumerToltalLs.Count != 0)
            {
                cashInConsumerToltal = cashInConsumerToltalLs.Select(c => c.MemberCashInCash).Aggregate((r, s) => (r + s));
            }

            var cashInDepositToltalLs = db.DepositOrderLogModels.Where(c => c.MemberId == member.Id.ToString()).ToList();
            int cashInDepositToltal = 0;
            if (cashInDepositToltalLs.Count != 0)
            {
                cashInDepositToltal = cashInDepositToltalLs.Select(c => c.MemberCashInCash).Aggregate((r, s) => (r + s));
            }

            memberOut = member;
            return (MemberSaveToltal - cashInConsumerToltal - cashInDepositToltal);
        }

        public ActionResult MemberDataBase()
        {
            if (HttpContext.User.Claims.Count() == 0) return RedirectToAction("Index", "Home");
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;

            var db = new CASE20210405Context();

            MemberModel member = new MemberModel();

            int currentCash = CurrentCash(getGmail, out member);

            MarkMemberDataBase Model = new MarkMemberDataBase
            {
                MemberId = member.Id,
                MemberGmail = member.MemberGmail,
                MemberName = member.MemberName,
                MemberIden = DESFunction.DESDecrypt(member.MemberIden, ProjectSet.PASSWORDKEY),
                MemberPhone = DESFunction.DESDecrypt(member.MemberPhone, ProjectSet.PASSWORDKEY),
                MemberCompany = member.MemberCompany,
                MemberMoney = currentCash
            };
            return View(Model);
        }
        [HttpPost]
        public ActionResult MemberEdit(MarkMemberDataBase it)
        {
            var db = new CASE20210405Context();
            var model = db.MemberModels.FirstOrDefault(c => c.MemberGmail == it.MemberGmail);
            if (model == null) return RedirectToAction("Logout", "Home");
            model.MemberIden = DESFunction.DESDecrypt(model.MemberIden, ProjectSet.PASSWORDKEY);
            model.MemberPhone = DESFunction.DESDecrypt(model.MemberPhone, ProjectSet.PASSWORDKEY);
            return View(model);
        }

        [HttpPost]
        public ActionResult MemberEditEnd(MemberModel it)
        {
            var db = new CASE20210405Context();

            if (ModelState.IsValid)
            {
                it.MemberIden = DESFunction.DESEncrypt(it.MemberIden, ProjectSet.PASSWORDKEY);
                it.MemberPhone = DESFunction.DESEncrypt(it.MemberPhone, ProjectSet.PASSWORDKEY);
                it.MemberChangeDate = DateTime.Now;
                db.Entry(it).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("MemberDataBase");
        }

        public ActionResult MemberProductList()
        {
            var db = new CASE20210405Context();
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            var getId = db.MemberModels.FirstOrDefault(c => c.MemberGmail == getGmail).Id;

            var modelLists = db.SalesOrderModels.Where(c => c.MemberId == getId.ToString()).ToList();
            ViewBag.ConsumerOrderLogs = db.ConsumerOrderLogModels.Where(c => c.MemberId == getId.ToString()).ToList();
            return View(modelLists);
        }

        [HttpPost]
        public string addDateTime(Guid id)
        {

            var db = new CASE20210405Context();
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            MemberModel member = new MemberModel();
            int currentCash = CurrentCash(getGmail, out member);

            var SaveOrder = db.SalesOrderModels.FirstOrDefault(c => c.Id == id);
            if ((currentCash - SaveOrder.ProductUnitPrice) < 0) return "餘額不足，延長失敗";

            var setDateTime = SaveOrder.ProductChangeDate.AddDays(SaveOrder.ProductDays);
            var CurrentDateTime = DateTime.Now;
            var ts = new TimeSpan(setDateTime.Ticks - CurrentDateTime.Ticks);
            var totalMinutes = ts.TotalMinutes;
            if (totalMinutes < 0)
            {
                SaveOrder.ProductChangeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                SaveOrder.ProductDays = SaveOrder.ProductUnitDays;
            }
            else
            {
                SaveOrder.ProductDays += SaveOrder.ProductUnitDays;
            }
            ConsumerOrderLogModel it = new ConsumerOrderLogModel()
            {
                MemberId = member.Id.ToString(),
                SalesOrderId = id.ToString(),
                MemberCashInCash = SaveOrder.ProductUnitPrice,
                MemberCashInDays = SaveOrder.ProductUnitDays,
                MemberCashInCrtDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                MemberCashInCounts = 0,
                MemberCashInRemarks1 = "",
                MemberCashInRemarks2 = ""
            };

            db.ConsumerOrderLogModels.Add(it);

            db.SaveChanges();
            return "OK";
        }

        [HttpPost]
        public string addCounts(Guid id,int totalCash)
        {
            var db = new CASE20210405Context();
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            MemberModel member = new MemberModel();
            int currentCash = CurrentCash(getGmail, out member);
            if (totalCash < 0) return "金額輸入有問題";
            if ((currentCash - totalCash) < 0) return "餘額不足，追加使用失敗";
            var SaveOrder = db.SalesOrderModels.FirstOrDefault(c => c.Id == id);

            ConsumerOrderLogModel it = new ConsumerOrderLogModel()
            {
                MemberId = member.Id.ToString(),
                SalesOrderId = id.ToString(),
                MemberCashInCash = totalCash,
                MemberCashInDays = 0,
                MemberCashInCrtDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                MemberCashInCounts = totalCash / SaveOrder.ProductUnitPrice,
                MemberCashInRemarks1 = "",
                MemberCashInRemarks2 = ""
            };

            db.ConsumerOrderLogModels.Add(it);

            db.SaveChanges();
            return "OK";
        }

        public ActionResult MemberProgressProductList()
        {
            var db = new CASE20210405Context();
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            var getId = db.MemberModels.FirstOrDefault(c => c.MemberGmail == getGmail).Id;

            var modelLists = db.MemberCashInModels.Where(c => c.MemberCashInId == getId.ToString()).ToList();
            ViewBag.DepositList = db.DepositOrderLogModels.Where(c => c.MemberId == getId.ToString()).ToList();
            return View(modelLists);
        }

        public string AddProgressProductMomey(Guid id, string GetMoney)
        {

            if (GetMoney == null) return "NG";
            if (!Regex.IsMatch(GetMoney, @"^[1-9][0-9]+$")) return "NG";
            var getMeoneyInt = Convert.ToInt32(GetMoney);
            if (getMeoneyInt < 100) return "金額必須大於100元以上";
            var db = new CASE20210405Context();
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            MemberModel member = new MemberModel();

            int currentCash = CurrentCash(getGmail, out member);

            if (currentCash - getMeoneyInt < 0) return "金額不夠，請加值!";

            DepositOrderLogModel it = new DepositOrderLogModel()
            {
                MemberId = member.Id.ToString(),
                CashInId = id.ToString(),
                MemberCashInCash = getMeoneyInt,
                MemberCashInCrtDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),

                MemberCashInRemarks1 = ""
            };

            db.DepositOrderLogModels.Add(it);
            db.SaveChanges();
            return "OK";
        }
        [HttpGet]
        public ActionResult AddProgressProductMomeyPage(Guid GId)
        {
            var db = new CASE20210405Context();
            var CashInModel = db.MemberCashInModels.Where(c => (c.Id == GId)).ToList();
            if(CashInModel.Count == 0 ) return RedirectToAction("MemberProgressProductList");
            ViewBag.CashInModel = CashInModel.FirstOrDefault();
            return View();
        }

        [HttpPost]
        public string checkMonery(int GetMoney)
        {
            if (GetMoney < 100) return "請輸入至少為100元之金額";
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            MemberModel member = new MemberModel();
            int currentCash = CurrentCash(getGmail, out member);
            if (currentCash - GetMoney < 0) return "餘額不足，請充值，謝謝您!";
            return "OK";
        }
        [HttpPost]
        public ActionResult AddProgressProductMomeyPage(DepositOrderLogModel it, string Remark)
        {

            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            MemberModel member = new MemberModel();
            int currentCash = CurrentCash(getGmail, out member);
            var Monery = currentCash - it.MemberCashInCash;
            if ( Monery < 0) return RedirectToAction("AddProgressProductMomeyPage", new { GId = it.CashInId });
            if (it.MemberCashInCash < 0) return RedirectToAction("AddProgressProductMomeyPage", new { GId = it.CashInId });
            var db = new CASE20210405Context();
            var CashInModelLs = db.MemberCashInModels.Where(c => c.Id.ToString() == it.CashInId && c.MemberCashInId == it.MemberId).ToList();
            if (CashInModelLs.Count == 0 ) return RedirectToAction("AddProgressProductMomeyPage", new { GId = it.CashInId });
            var CashInModel = CashInModelLs.FirstOrDefault();
            it.MemberCashInCrtDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            it.MemberCashInRemarks1 = Remark;
            CashInModel.MemberCashInChgDate = it.MemberCashInCrtDate;
            CashInModel.MemberCashInSpace1 = Remark;
            db.DepositOrderLogModels.Add(it);
            db.SaveChanges();
            return RedirectToAction("MemberProgressProductList");
        }

        public ActionResult AddProgressProduct()
        {
            return View();
        }
        [HttpPost]
        public RedirectToActionResult AddProgressProduct(MemberCashInModel it)
        {
            if (string.IsNullOrEmpty(it.MemberCashInName))
                return RedirectToAction("MemberProgressProductList");
            var db = new CASE20210405Context();
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            MemberModel member = db.MemberModels.Where(c => c.MemberGmail == getGmail).ToList().FirstOrDefault();
            it.MemberCashInId = member.Id.ToString();
            it.MemberCashInCrtDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:00"));
            it.MemberCashInChgDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:00"));
            db.MemberCashInModels.Add(it);
            db.SaveChanges();
            return RedirectToAction("MemberProgressProductList");
        }

        public ActionResult SaveCash()
        {
            return View();
        }

        public ActionResult DownLoadPage()
        {
            var db = new CASE20210405Context();
            var getGmail = HttpContext.User.Claims.ToList()[2].Value;
            MemberModel member = db.MemberModels.FirstOrDefault(c => c.MemberGmail == getGmail);
            if (member == null) return RedirectToAction("SaveCash");
            var FileLoadmodels = db.FileLoadInfoModels.Where(c => c.MmberId == member.Id.ToString()).ToList();
            return View(FileLoadmodels);
        }

        public ActionResult Question()
        {
            return View();
        }
    }
}
