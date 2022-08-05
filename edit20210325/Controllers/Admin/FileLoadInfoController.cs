using edit20210325.Function;
using edit20210325.Models;
using edit20210325.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Controllers.Admin
{
    [Area(ProjectSet.AdminName)]
    public class FileLoadInfoController : Controller
    {
        const int pageRowCount = 20;
        private readonly string localPath;
        public FileLoadInfoController(IWebHostEnvironment env)
        {
            localPath = env.ContentRootPath;
        }
        public IActionResult Index(string search = "", string searchSelect = "1", int page = 1, bool FuzzyOrNormal = false)
        {
            if (HttpContext.User.Claims.Count() == 0) return RedirectToAction("Index", "Login");
            var db = new CASE20210405Context();

            List<FileLoadInfoModel> lsSourceModels = db.FileLoadInfoModels.ToList();

            List<MemberModel> ModelList = db.MemberModels.ToList();

            List<MarkFileLoadInfoModel> lsModelout = ModelList.Join(
                lsSourceModels,
                m => Convert.ToString(m.Id),
                s => s.MmberId,
                (m, s) => new MarkFileLoadInfoModel()
                {
                    Id = s.Id,
                    MemberGmail = m.MemberGmail,
                    MemberName = m.MemberName,
                    FileTitle = s.FileTitle,
                    FileUrl = s.FileUrl,
                    FileUpDateTime = s.FileUpDateTime
                }).OrderBy(c => c.Id).ToList();

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
            List<MarkFileLoadInfoModel> lsModelout = JsonConvert.DeserializeObject<List<MarkFileLoadInfoModel>>(strlsModel);

            if (search == null) search = "";
            List<MarkFileLoadInfoModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.MemberGmail.Equals(search)).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.MemberName.Equals(search)).ToList();
                    break;
                case "3":
                    serachModelout = lsModelout.Where(c => c.FileTitle.Equals(search)).ToList();
                    break;
                case "4":
                    serachModelout = lsModelout.Where(c => c.FileUpDateTime.ToString("yyyy/MM/dd HH:mm").Equals(search)).ToList();
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
            List<MarkFileLoadInfoModel> lsModelout = JsonConvert.DeserializeObject<List<MarkFileLoadInfoModel>>(strlsModel);

            if (search == null) search = "";
            List<MarkFileLoadInfoModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.MemberGmail.IndexOf(search) > -1).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.MemberName.IndexOf(search) > -1).ToList();
                    break;
                case "3":
                    serachModelout = lsModelout.Where(c => c.FileTitle.IndexOf(search) > -1).ToList();
                    break;
                case "4":
                    serachModelout = lsModelout.Where(c => c.FileUpDateTime.ToString("yyyy/MM/dd HH:mm").IndexOf(search) > -1).ToList();
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
            var db = new CASE20210405Context();
            List<MemberModel> ModelList = db.MemberModels.ToList();
            ViewBag.ModelList = ModelList;
            return View();
        }

        [HttpPost]
        public RedirectToActionResult Create(FileLoadInfoModel it)
        {
            if (string.IsNullOrEmpty(it.FileTitle))
                return RedirectToAction("Index");
            var db = new CASE20210405Context();
            it.FileMac = "FF-FF-FF-FF-FF-FF";
            db.FileLoadInfoModels.Add(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="path"></param>
        public void deleteFile(string Url, string path)
        {
            var fileName = Path.GetFileName(Url);
            var pathFile = localPath + "\\wwwroot\\" + path + "\\" + fileName;
            if (System.IO.File.Exists(pathFile))
            {
                try
                {
                    System.IO.File.Delete(pathFile);
                }
                catch (Exception) { }

            }
        }
        public RedirectToActionResult Delete(Guid? Id)
        {
            var db = new CASE20210405Context();
            FileLoadInfoModel it = db.FileLoadInfoModels.Find(Id);
            db.FileLoadInfoModels.Remove(it);
            db.SaveChanges();
            deleteFile(it.FileUrl, "files");
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid? Id)
        {
            if (Id == null) return RedirectToAction("Index");
            var db = new CASE20210405Context();
            FileLoadInfoModel it = db.FileLoadInfoModels.Find(Id);
            if(it == null) return RedirectToAction("Index");
            var member =  db.MemberModels.Find(Guid.Parse(it.MmberId));
            if(member == null ) return RedirectToAction("Index");
            ViewBag.member = member;

            return View(it);
        }

        public static string GetRandomStringByFileName(int length)
        {
            var str = Path.GetRandomFileName().Replace(".", "");
            return str.Substring(0, length);
        }
        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 34359738368)]
        public string Edit(string item, IFormFile file)
        {
            var db = new CASE20210405Context();
            var FileParh = localPath + "\\wwwroot\\files\\";

            FileLoadInfoModel Modelout = JsonConvert.DeserializeObject<FileLoadInfoModel>(item);
            Modelout.FileUpDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            if (file != null)
            {
                deleteFile(Modelout.FileUrl, "files");
                var fileExtension = GetRandomStringByFileName(10) + ".zip";
                Modelout.FileUrl = "/files/" + fileExtension;
                if (!Directory.Exists(FileParh))
                {
                    Directory.CreateDirectory(FileParh);
                }
                using (var fileStream = new FileStream(Path.Combine(FileParh + fileExtension), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(Modelout).State = EntityState.Modified;
                db.SaveChanges();
            }
            return "OK";
        }

        [HttpPost]
        public string ClearFile()
        {
            var videoParh = localPath + "\\wwwroot\\files\\";
            deleteFileFunc(videoParh, "files");
            return "清除完成";
        }
        public void deleteFileFunc(string path, string pathData)
        {
            var db = new CASE20210405Context();
            var lsModel = db.FileLoadInfoModels.ToList();
            var files = Directory.GetFiles(path).ToList();

            foreach (var it in lsModel)
            {
                string fileName = "";
                fileName = Path.GetFileName(it.FileUrl);
                for (int i = 0; i < files.Count; i++)
                {
                    if (Path.GetFileName(files[i]).Equals(fileName))
                    {
                        files.RemoveAt(i);
                    }
                }
            }
            foreach (var file in files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

    }


}
