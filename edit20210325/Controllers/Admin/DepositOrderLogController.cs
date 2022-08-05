using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edit20210325.Models;
using edit20210325.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace edit20210325.Controllers.Admin
{
    [Area(Function.ProjectSet.AdminName)]
    public class DepositOrderLogController : Controller
    {
        const int pageRowCount = 20;
        public IActionResult Index(string search = "", string searchSelect = "1", int page = 1, bool FuzzyOrNormal = false)
        {
            if (HttpContext.User.Claims.Count() == 0) return RedirectToAction("Index", "Login");
            var db = new CASE20210405Context();

            List<DepositOrderLogModel> lsSourceModels = db.DepositOrderLogModels.ToList();
            List<MemberCashInModel> lsMemberCashInModels = db.MemberCashInModels.ToList();

            List<MemberModel> ModelList = db.MemberModels.ToList();

            List<MarkDepositOrderLogModel> lsModelout = ModelList.Join(
                lsSourceModels,
                m => Convert.ToString(m.Id),
                s => s.MemberId,
                (m, s) => new
                {
                    Id = s.Id,
                    MemberEmail = m.MemberGmail,
                    MemberName = m.MemberName,
                    MemberCash = s.MemberCashInCash,
                    CaseId = s.CashInId,
                    CaseRemarks1 = s.MemberCashInRemarks1,
                    CaseRemarks2 = s.MemberCashInRemarks2,
                    CreateDate = s.MemberCashInCrtDate,
                }).Join(
                lsMemberCashInModels,
                c => c.CaseId,
                r => Convert.ToString(r.Id),
                (c, r) => new MarkDepositOrderLogModel()
                {
                    Id = c.Id,
                    MemberEmail = c.MemberEmail,
                    MemberName = c.MemberName,
                    MemberCash = c.MemberCash,
                    CaseName = r.MemberCashInName,
                    CaseRemarks1 = c.CaseRemarks1,
                    CaseRemarks2 = c.CaseRemarks2,
                    CreateDate = c.CreateDate
                }
                ).OrderBy(c => c.Id).ToList();

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
            List<MarkDepositOrderLogModel> lsModelout = JsonConvert.DeserializeObject<List<MarkDepositOrderLogModel>>(strlsModel);

            if (search == null) search = "";
            List<MarkDepositOrderLogModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.MemberEmail.Equals(search)).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.MemberName.Equals(search)).ToList();
                    break;
                case "3":
                    serachModelout = lsModelout.Where(c => c.MemberCash.ToString().Equals(search)).ToList();
                    break;
                case "4":
                    serachModelout = lsModelout.Where(c => c.CaseName.Equals(search)).ToList();
                    break;
                case "5":
                    serachModelout = lsModelout.Where(c => c.CaseRemarks1.Equals(search)).ToList();
                    break;
                case "6":
                    serachModelout = lsModelout.Where(c => c.CaseRemarks2.Equals(search)).ToList();
                    break;
                case "7":
                    serachModelout = lsModelout.Where(c => c.CreateDate.Date.ToString("yyyy/MM/dd").Equals(search)).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.MemberEmail.Equals(search)).ToList();
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
            List<MarkDepositOrderLogModel> lsModelout = JsonConvert.DeserializeObject<List<MarkDepositOrderLogModel>>(strlsModel);

            if (search == null) search = "";
            List<MarkDepositOrderLogModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.MemberEmail.IndexOf(search) > -1).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.MemberName.IndexOf(search) > -1).ToList();
                    break;
                case "3":
                    serachModelout = lsModelout.Where(c => c.MemberCash.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "4":
                    serachModelout = lsModelout.Where(c => c.CaseName.IndexOf(search) > -1).ToList();
                    break;
                case "5":
                    serachModelout = lsModelout.Where(c => c.CaseRemarks1.IndexOf(search) > -1).ToList();
                    break;
                case "6":
                    serachModelout = lsModelout.Where(c => c.CaseRemarks2.IndexOf(search) > -1).ToList();
                    break;
                case "7":
                    serachModelout = lsModelout.Where(c => c.CreateDate.Date.ToString("yyyy/MM/dd").IndexOf(search) > -1).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.MemberEmail.IndexOf(search) > -1).ToList();
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
            var db = new CASE20210405Context();
            List<MemberModel> ModelList = db.MemberModels.ToList();
            ViewBag.ModelList = ModelList;
            List<MemberCashInModel> CaseInIds = db.MemberCashInModels.ToList();
            ViewBag.CaseInIds = CaseInIds;
            return View();
        }
        [HttpPost]
        public RedirectToActionResult Create(DepositOrderLogModel it)
        {
            if (string.IsNullOrEmpty(it.MemberId))
                return RedirectToAction("Index");
            var db = new CASE20210405Context();
            db.DepositOrderLogModels.Add(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public RedirectToActionResult Delete(Guid? Id)
        {
            var db = new CASE20210405Context();
            DepositOrderLogModel it = db.DepositOrderLogModels.Find(Id);
            db.DepositOrderLogModels.Remove(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid? Id)
        {
            if (Id == null) return RedirectToAction("Index");

            var db = new CASE20210405Context();
            List<DepositOrderLogModel> lsModel = db.DepositOrderLogModels.ToList();
            List<DepositOrderLogModel> ls = lsModel.Where(c => c.Id == Id).ToList();
            if (ls.Count == 0) return RedirectToAction("Index");
            DepositOrderLogModel it = ls.FirstOrDefault();

            List<MemberModel> memberList = db.MemberModels.Where(c => c.Id.ToString() == it.MemberId).ToList();
            if (memberList.Count == 0) return RedirectToAction("Index");
            var member = memberList.FirstOrDefault();
            ViewBag.member = member;

            List<MemberCashInModel> MemberCashInList = db.MemberCashInModels.Where(c => c.Id.ToString() == it.CashInId).ToList();
            if (MemberCashInList.Count == 0) return RedirectToAction("Index");
            var CashIn = MemberCashInList.FirstOrDefault();
            ViewBag.CashIn = CashIn;

            return View(it);
        }

        [HttpPost]
        public RedirectToActionResult Edit(DepositOrderLogModel it)
        {
            var db = new CASE20210405Context();

            if (ModelState.IsValid)
            {
                db.Entry(it).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
