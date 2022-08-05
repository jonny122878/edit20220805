using edit20210325.Function;
using edit20210325.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Controllers
{
    [Area(ProjectSet.AdminName)]
    public class ManagerController : Controller
    {
        const int pageRowCount = 20;
        public IActionResult Index(string search = "", string searchSelect = "1", int page = 1, bool FuzzyOrNormal = false)
        {
            if (HttpContext.User.Claims.Count() == 0) return RedirectToAction("Index", "Login");
            var db = new CASE20210405Context();
            List<ManagerModel> lsModelout = db.ManagerModels.ToList();
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
            List<ManagerModel> lsModelout = JsonConvert.DeserializeObject<List<ManagerModel>>(strlsModel);

            if (search == null) search = "";
            List<ManagerModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.ManagerUser.Equals(search)).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.ManagerName.Equals(search)).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.ManagerUser.Equals(search)).ToList();
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
            List<ManagerModel> lsModelout = JsonConvert.DeserializeObject<List<ManagerModel>>(strlsModel);

            if (search == null) search = "";
            List<ManagerModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.ManagerUser.IndexOf(search) > -1).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.ManagerName.IndexOf(search) > -1).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.ManagerUser.IndexOf(search) > -1).ToList();
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
        public RedirectToActionResult Create(ManagerModel it)
        {
            if (string.IsNullOrEmpty(it.ManagerName))
                return RedirectToAction("Create");

            var db = new CASE20210405Context();
            List<ManagerModel> lsModel = db.ManagerModels.ToList();
            ManagerModel myIt = lsModel.Where(c => c.ManagerUser == it.ManagerUser).FirstOrDefault();
            if(myIt!= null)
            {
                TempData["Error"] = "請注意，帳號重複!!";
                return RedirectToAction("Create"); 
            }
            db.ManagerModels.Add(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public RedirectToActionResult Delete(Guid? Id)
        {
            var db = new CASE20210405Context();
            ManagerModel it = db.ManagerModels.Find(Id);
            db.ManagerModels.Remove(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(Guid? Id)
        {
            if (Id == null) return RedirectToAction("Index");

            var db = new CASE20210405Context();
            List<ManagerModel> lsModel = db.ManagerModels.ToList();
            List<ManagerModel> ls = lsModel.Where(c => c.Id == Id).ToList();
            if (ls.Count == 0) return RedirectToAction("Index");
            ManagerModel it = ls.FirstOrDefault();
            return View(it);
        }
        [HttpPost]
        public RedirectToActionResult Edit(ManagerModel it)
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
