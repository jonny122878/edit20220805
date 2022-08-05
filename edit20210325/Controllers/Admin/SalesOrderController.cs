using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using edit20210325.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using edit20210325.ViewModels;
using edit20210325.Function;

namespace edit20210325.Controllers
{
    [Area(ProjectSet.AdminName)]
    public class SalesOrderController : Controller
    {
        const int pageRowCount = 20;

        public ActionResult Index(string search = "", string searchSelect = "1", int page = 1,bool FuzzyOrNormal = false )
        {
            if (HttpContext.User.Claims.Count() == 0) return RedirectToAction("Index", "Login");
            var db = new CASE20210405Context();

            List<SalesOrderModel> lsSourceModels = db.SalesOrderModels.ToList();

            List<MemberModel> ModelList = db.MemberModels.ToList();

            List<MarkSalesOrderModel> lsModelout = ModelList.Join(
                lsSourceModels,
                m => Convert.ToString(m.Id),
                s => s.MemberId,
                (m, s) => new MarkSalesOrderModel()
                {
                    Id = s.Id,
                    MemberEmail = m.MemberGmail,
                    MemberName = m.MemberName,
                    ProductName = s.ProductName,
                    ProductInfo = s.ProductInfo,
                    ProductCharge = s.ProductCharge,
                    ProductCounts = s.ProductCounts,
                    ProductUnitDays = s.ProductUnitDays,
                    ProductUnitPrice = s.ProductUnitPrice,
                    ProductRemarks1 = s.ProductRemarks1,
                    ProductRemarks2 = s.ProductRemarks2,
                    ProductDays = s.ProductDays,
                    ProductCreateDate = s.ProductCreateDate,
                    ProductChangeDate = s.ProductChangeDate
                }).OrderBy(c => c.Id).ToList();


            var pageCnt = page;
            var pageRows = pageRowCount;
            var querylsModel = lsModelout.Skip((pageCnt - 1) * pageRows).Take(pageRows);
            int listConut = (lsModelout.Count / pageRowCount) + 1;
            if (lsModelout.Count % pageRowCount > 0) listConut += 1;
            var pageNext = pageCnt + 1;
            if (pageNext >= listConut - 1) pageNext = listConut - 1;
            var pagePrev = pageCnt -1;
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
                listConutEnd = page + pageRowsQuantity-1;
            }
            if (listConutStart <= 1) listConutStart = 1;
            if (listConutEnd > listConut) listConutEnd = listConut-1;
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
        
        public ActionResult SearchView(string search="" , string searchSelect="1", int page = 1)
        {
            string strlsModel = (string)TempData["save"];
            List<MarkSalesOrderModel> lsModelout = JsonConvert.DeserializeObject<List<MarkSalesOrderModel>>(strlsModel);
          
            if (search == null) search = "";
            List<MarkSalesOrderModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.MemberEmail.Equals(search)).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.MemberName.Equals(search)).ToList();
                    break;
                case "3":
                    serachModelout = lsModelout.Where(c => c.ProductName.Equals(search)).ToList();
                    break;
                case "4":
                    serachModelout = lsModelout.Where(c => c.ProductInfo.Equals(search)).ToList();
                    break;
                case "5":
                    serachModelout = lsModelout.Where(c => c.ProductCharge.Equals(search)).ToList();
                    break;
                case "6":
                    serachModelout = lsModelout.Where(c => c.ProductCounts.ToString().Equals(search)).ToList();
                    break;
                case "7":
                    serachModelout = lsModelout.Where(c => c.ProductUnitDays.ToString().Equals(search)).ToList();
                    break;
                case "8":
                    serachModelout = lsModelout.Where(c => c.ProductUnitPrice.ToString().Equals(search)).ToList();
                    break;
                case "9":
                    serachModelout = lsModelout.Where(c => c.ProductRemarks1.Equals(search)).ToList();
                    break;
                case "10":
                    serachModelout = lsModelout.Where(c => c.ProductRemarks2.Equals(search)).ToList();
                    break;
                case "11":
                    serachModelout = lsModelout.Where(c => c.ProductDays.ToString().Equals(search)).ToList();
                    break;
                case "12":
                    serachModelout = lsModelout.Where(c => c.ProductCreateDate.Date.ToString("yyyy/MM/dd").Equals(search)).ToList();
                    break;
                case "13":
                    serachModelout = lsModelout.Where(c => c.ProductChangeDate.ToString("yyyy/MM/dd HH:mm").Equals(search)).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.ProductName.Equals(search)).ToList();
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

        public ActionResult FuzzySearchView(string search="", string searchSelect="1", int page = 1)
        {

            string strlsModel = (string)TempData["save"];
            List<MarkSalesOrderModel> lsModelout = JsonConvert.DeserializeObject<List<MarkSalesOrderModel>>(strlsModel);
  
            if (search == null) search = "";         
            List<MarkSalesOrderModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.MemberEmail.IndexOf(search) > -1).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.MemberName.IndexOf(search) > -1).ToList();
                    break;
                case "3":
                    serachModelout = lsModelout.Where(c => c.ProductName.IndexOf(search) > -1).ToList();
                    break;
                case "4":
                    serachModelout = lsModelout.Where(c => c.ProductInfo.IndexOf(search) > -1).ToList();
                    break;
                case "5":
                    serachModelout = lsModelout.Where(c => c.ProductCharge.IndexOf(search) > -1).ToList();
                    break;
                case "6":
                    serachModelout = lsModelout.Where(c => c.ProductCounts.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "7":
                    serachModelout = lsModelout.Where(c => c.ProductUnitDays.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "8":
                    serachModelout = lsModelout.Where(c => c.ProductUnitPrice.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "9":
                    serachModelout = lsModelout.Where(c => c.ProductRemarks1.IndexOf(search) > -1).ToList();
                    break;
                case "10":
                    serachModelout = lsModelout.Where(c => c.ProductRemarks2.IndexOf(search) > -1).ToList();
                    break;
                case "11":
                    serachModelout = lsModelout.Where(c => c.ProductDays.ToString().IndexOf(search) > -1).ToList();
                    break;
                case "12":
                    serachModelout = lsModelout.Where(c => c.ProductCreateDate.Date.ToString("yyyy/MM/dd").IndexOf(search) > -1).ToList();
                    break;
                case "13":
                    serachModelout = lsModelout.Where(c => c.ProductChangeDate.ToString("yyyy/MM/dd HH:mm").IndexOf(search) > -1).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.ProductName.IndexOf(search) > -1).ToList();
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
            return View();
        }
        [HttpPost]
        public RedirectToActionResult Create(SalesOrderModel it)
        {
            if (string.IsNullOrEmpty(it.ProductName))
                        return RedirectToAction("Index");
            var db = new CASE20210405Context();
            db.SalesOrderModels.Add(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public RedirectToActionResult Delete(Guid? Id)
        {
            var db = new CASE20210405Context();
            SalesOrderModel it = db.SalesOrderModels.Find(Id);
            db.SalesOrderModels.Remove(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid? Id)
        {
            if (Id == null) return RedirectToAction("Index");

            var db = new CASE20210405Context();

            List<SalesOrderModel> lsModel = db.SalesOrderModels.ToList();
            List<SalesOrderModel> ls = lsModel.Where(c => c.Id == Id).ToList();
            if (ls.Count == 0) return RedirectToAction("Index");
            SalesOrderModel it = ls.FirstOrDefault();

            List<MemberModel> memberList = db.MemberModels.Where(c => c.Id.ToString() == it.MemberId).ToList();
            if (memberList.Count == 0) return RedirectToAction("Index");
            var member = memberList.FirstOrDefault();
            ViewBag.member = member;

            return View(it);
        }
        [HttpPost]
        public RedirectToActionResult Edit(SalesOrderModel it)
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
