using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edit20210325.Function;
using edit20210325.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace edit20210325.Controllers
{
    [Area(ProjectSet.AdminName)]
    public class MemberController : Controller
    {
        const int pageRowCount = 20;
        public IActionResult Index(string search = "", string searchSelect = "1", int page = 1, bool FuzzyOrNormal = false)
        {
            if (HttpContext.User.Claims.Count() == 0) return RedirectToAction("Index", "Login");
            var db = new CASE20210405Context();
            List<MemberModel> lsModelout = db.MemberModels.ToList();
            var pageCnt = page;
            var pageRows = pageRowCount;
            var querylsModel = lsModelout.Skip((pageCnt - 1) * pageRows).Take(pageRows);
            int listConut = (lsModelout.Count / pageRowCount) + 1;
            if (lsModelout.Count % pageRowCount > 0) listConut += 1;
            var pageNext = pageCnt + 1;
            if (pageNext >= listConut - 1) pageNext = listConut - 1;
            var pagePrev = pageCnt - 1;
            if (pagePrev <= 1) pagePrev = 1;
            ViewData["pageNext"] = pageNext;
            ViewData["pagePrev"] = pagePrev;
            int listConutStart = 0;
            int listConutEnd = 10;
            var pageRowsQuantity = 5;
            if (page <= pageRowsQuantity)
            {
                listConutStart = 1;
                listConutEnd = 10;
            }
            else
            {
                listConutStart = page - pageRowsQuantity;
                listConutEnd = page + pageRowsQuantity - 1;
            }
            if (listConutStart <= 1) listConutStart = 1;
            if (listConutEnd > listConut) listConutEnd = listConut - 1;
            ViewData["listConut"] = listConut;
            ViewData["page"] = page;
            ViewData["listConutStart"] = listConutStart;
            ViewData["listConutEnd"] = listConutEnd;
            ViewData["CountModel"] = querylsModel.Count();
            TempData["save"] = JsonConvert.SerializeObject(lsModelout);
            ViewData["search"] = search;
            ViewData["searchSelect"] = searchSelect;
            ViewData["FuzzyOrNormal"] = FuzzyOrNormal;
            return View(querylsModel);
        }

        public ActionResult SearchView(string search = "", string searchSelect = "1", int page = 1)
        {
            string strlsModel = (string)TempData["save"];
            List<MemberModel> lsModelout = JsonConvert.DeserializeObject<List<MemberModel>>(strlsModel);

            if (search == null) search = "";
            List<MemberModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.MemberGmail.Equals(search)).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.MemberName.Equals(search)).ToList();
                    break;
                case "3":
                    serachModelout = lsModelout.Where(c => c.MemberIden.ToString().Equals(search)).ToList();
                    break;
                case "4":
                    serachModelout = lsModelout.Where(c => c.MemberPhone.ToString().Equals(search)).ToList();
                    break;
                case "5":
                    serachModelout = lsModelout.Where(c => c.MemberCompany.Equals(search)).ToList();
                    break;
                case "6":
                    serachModelout = lsModelout.Where(c => c.MemberRemarks1.ToString().Equals(search)).ToList();
                    break;
                case "7":
                    serachModelout = lsModelout.Where(c => c.MemberRemarks2.ToString().Equals(search)).ToList();
                    break;
                case "8":
                    serachModelout = lsModelout.Where(c => c.MemberSpace1.ToString().Equals(search)).ToList();
                    break;
                case "9":
                    serachModelout = lsModelout.Where(c => c.MemberSpace2.Equals(search)).ToList();
                    break;
                case "10":
                    serachModelout = lsModelout.Where(c => c.MemberCreateDate.Date.ToString("yyyy/MM/dd").Equals(search)).ToList();
                    break;
                case "11":
                    serachModelout = lsModelout.Where(c => c.MemberChangeDate.Date.ToString("yyyy/MM/dd").Equals(search)).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.MemberGmail.Equals(search)).ToList();
                    break;
            }

            string JsonDataout = JsonConvert.SerializeObject(serachModelout);
            TempData["save"] = JsonDataout;
            ViewData["search"] = search;
            ViewData["searchSelect"] = searchSelect;
            ViewData["FuzzyOrNormal"] = true;
            var pageCnt = page;
            var pageRows = pageRowCount;
            var querylsModel = serachModelout.Skip((pageCnt - 1) * pageRows).Take(pageRows);
            int listConut = (serachModelout.Count / pageRowCount) + 1;
            if (serachModelout.Count % pageRowCount > 0) listConut += 1;
            var pageNext = pageCnt + 1;
            if (pageNext >= listConut - 1) pageNext = listConut - 1;
            var pagePrev = pageCnt - 1;
            if (pagePrev <= 1) pagePrev = 1;
            ViewData["pageNext"] = pageNext;
            ViewData["pagePrev"] = pagePrev;
            int listConutStart = 0;
            int listConutEnd = 10;
            var pageRowsQuantity = 5;
            if (page <= pageRowsQuantity)
            {
                listConutStart = 1;
                listConutEnd = 10;
            }
            else
            {
                listConutStart = page - pageRowsQuantity;
                listConutEnd = page + pageRowsQuantity - 1;
            }
            if (listConutStart <= 1) listConutStart = 1;
            if (listConutEnd > listConut) listConutEnd = listConut - 1;
            ViewData["listConut"] = listConut;
            ViewData["page"] = page;
            ViewData["listConutStart"] = listConutStart;
            ViewData["listConutEnd"] = listConutEnd;
            ViewData["CountModel"] = querylsModel.Count();
            return View(querylsModel);
        }

        public ActionResult FuzzySearchView(string search = "", string searchSelect = "1", int page = 1)
        {

            string strlsModel = (string)TempData["save"];
            List<MemberModel> lsModelout = JsonConvert.DeserializeObject<List<MemberModel>>(strlsModel);

            if (search == null) search = "";
            List<MemberModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.MemberGmail.IndexOf(search) > -1 ).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.MemberName.IndexOf(search) > -1).ToList();
                    break;
                case "3":
                    serachModelout = lsModelout.Where(c => c.MemberIden.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "4":
                    serachModelout = lsModelout.Where(c => c.MemberPhone.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "5":
                    serachModelout = lsModelout.Where(c => c.MemberCompany.IndexOf(search) > -1).ToList();
                    break;
                case "6":
                    serachModelout = lsModelout.Where(c => c.MemberRemarks1.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "7":
                    serachModelout = lsModelout.Where(c => c.MemberRemarks2.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "8":
                    serachModelout = lsModelout.Where(c => c.MemberSpace1.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "9":
                    serachModelout = lsModelout.Where(c => c.MemberSpace2.IndexOf(search) > -1).ToList();
                    break;
                case "10":
                    serachModelout = lsModelout.Where(c => c.MemberCreateDate.Date.ToString("yyyy/MM/dd").IndexOf(search) > -1).ToList();
                    break;
                case "11":
                    serachModelout = lsModelout.Where(c => c.MemberChangeDate.Date.ToString("yyyy/MM/dd").IndexOf(search) > -1).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.MemberGmail.IndexOf(search) > -1).ToList();
                    break;
            }

            string JsonDataout = JsonConvert.SerializeObject(serachModelout);
            TempData["save"] = JsonDataout;
            ViewData["search"] = search;
            ViewData["searchSelect"] = searchSelect;
            ViewData["FuzzyOrNormal"] = false;
            var pageCnt = page;
            var pageRows = pageRowCount;
            var querylsModel = serachModelout.Skip((pageCnt - 1) * pageRows).Take(pageRows);
            int listConut = (serachModelout.Count / pageRowCount) + 1;
            if (serachModelout.Count % pageRowCount > 0) listConut += 1;
            var pageNext = pageCnt + 1;
            if (pageNext >= listConut - 1) pageNext = listConut - 1;
            var pagePrev = pageCnt - 1;
            if (pagePrev <= 1) pagePrev = 1;
            ViewData["pageNext"] = pageNext;
            ViewData["pagePrev"] = pagePrev;
            int listConutStart = 0;
            int listConutEnd = 10;
            var pageRowsQuantity = 5;
            if (page <= pageRowsQuantity)
            {
                listConutStart = 1;
                listConutEnd = 10;
            }
            else
            {
                listConutStart = page - pageRowsQuantity;
                listConutEnd = page + pageRowsQuantity - 1;
            }
            if (listConutStart <= 1) listConutStart = 1;
            if (listConutEnd > listConut) listConutEnd = listConut - 1;
            ViewData["listConut"] = listConut;
            ViewData["page"] = page;
            ViewData["listConutStart"] = listConutStart;
            ViewData["listConutEnd"] = listConutEnd;
            ViewData["CountModel"] = querylsModel.Count();
            return View(querylsModel);
        }

        public RedirectToActionResult exit()
        {
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public RedirectToActionResult Create(MemberModel it)
        {
            if (string.IsNullOrEmpty(it.MemberName))
                return RedirectToAction("Index");
            var db = new CASE20210405Context();
            it.MemberIden = DESFunction.DESEncrypt(it.MemberIden, ProjectSet.PASSWORDKEY);
            it.MemberPhone = DESFunction.DESEncrypt(it.MemberPhone, ProjectSet.PASSWORDKEY);
            db.MemberModels.Add(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public RedirectToActionResult Delete(Guid? Id)
        {
            var db = new CASE20210405Context();
            MemberModel it = db.MemberModels.Find(Id);
            db.MemberModels.Remove(it);

            List<SalesOrderModel> lsModel1 = db.SalesOrderModels.Where(c =>c.MemberId == Id.ToString()).ToList();
            db.SalesOrderModels.RemoveRange(lsModel1);
            List<MemberSaveModel> lsModel2 = db.MemberSaveModels.Where(c => c.MemberSaveId == Id.ToString()).ToList();
            db.MemberSaveModels.RemoveRange(lsModel2);
            List<MemberCashInModel> lsModel3 = db.MemberCashInModels.Where(c => c.MemberCashInId == Id.ToString()).ToList();
            db.MemberCashInModels.RemoveRange(lsModel3);
            List<FileLoadInfoModel> lsModel4 = db.FileLoadInfoModels.Where(c => c.MmberId == Id.ToString()).ToList();
            db.FileLoadInfoModels.RemoveRange(lsModel4);

            var ConsumerOrderLogs = db.ConsumerOrderLogModels.Where(c => c.MemberId == Id.ToString()).ToList();
            db.ConsumerOrderLogModels.RemoveRange(ConsumerOrderLogs);

            var DepositOrderLogs = db.DepositOrderLogModels.Where(c => c.MemberId == Id.ToString()).ToList();
            db.DepositOrderLogModels.RemoveRange(DepositOrderLogs);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(Guid? Id)
        {
            if (Id == null) return RedirectToAction("Index");
            var db = new CASE20210405Context();

            List<MemberModel> lsModel = db.MemberModels.ToList();
            List<MemberModel> ls = lsModel.Where(c => c.Id == Id).ToList();
            if (ls.Count == 0) return RedirectToAction("Index");
            MemberModel it = ls.FirstOrDefault();
            it.MemberIden = DESFunction.DESDecrypt(it.MemberIden, ProjectSet.PASSWORDKEY);
            it.MemberPhone = DESFunction.DESDecrypt(it.MemberPhone, ProjectSet.PASSWORDKEY);
            return View(it);
        }
        [HttpPost]
        public RedirectToActionResult Edit(MemberModel it)
        {
            var db = new CASE20210405Context();

            if (ModelState.IsValid)
            {
                it.MemberIden = DESFunction.DESEncrypt(it.MemberIden, ProjectSet.PASSWORDKEY);
                it.MemberPhone = DESFunction.DESEncrypt(it.MemberPhone, ProjectSet.PASSWORDKEY);
                db.Entry(it).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
