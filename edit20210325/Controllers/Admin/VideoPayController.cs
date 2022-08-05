using edit20210325.Function;
using edit20210325.Models;
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

namespace edit20210325.Controllers
{
    [Area(ProjectSet.AdminName)]
    public class VideoPayController : Controller
    {
        const int pageRowCount = 20;
        private readonly string localPath;
        public VideoPayController(IWebHostEnvironment env)
        {
            localPath = env.ContentRootPath;
        }

        public static string GetRandomStringByFileName(int length)
        {
            var str = Path.GetRandomFileName().Replace(".", "");
            return str.Substring(0, length);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 34359738368)]
        public string updateFileImage(IFormFile file)
        {
            if (file == null) return "傳送失敗";
            var FileParh = localPath + "\\wwwroot\\PayVideoImage\\";
            var ImgExtension = GetRandomStringByFileName(10) + ".jpg";
            if (!Directory.Exists(FileParh))
            {
                Directory.CreateDirectory(FileParh);
            }
            using (var fileStream = new FileStream(Path.Combine(FileParh + ImgExtension), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            return "/PayVideoImage/" + ImgExtension;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 34359738368)]
        public string changFileImage(IFormFile file ,string Id)
        {
            if (file == null) return "傳送失敗";

            if (Id == null) return "傳送失敗";
            var db = new CASE20210405Context();
            List<VideoPayModel> lsModel = db.VideoPayModels.ToList();
            List<VideoPayModel> ls = lsModel.Where(c => c.Id.ToString() == Id).ToList();
            if (ls.Count == 0) return "傳送失敗";
            VideoPayModel it = ls.FirstOrDefault();
            
            var FileParh = localPath + "\\wwwroot\\PayVideoImage\\";
            var ImgExtension = GetRandomStringByFileName(10) + ".jpg";

            it.videoImageUrl = "/PayVideoImage/" + ImgExtension;
            if (ModelState.IsValid)
            {
                db.Entry(it).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (!Directory.Exists(FileParh))
            {
                Directory.CreateDirectory(FileParh);
            }
            using (var fileStream = new FileStream(Path.Combine(FileParh + ImgExtension), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            return "/PayVideoImage/" + ImgExtension;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 34359738368)]
        public string updateFileVideo(IFormFile file)
        {
            if (file == null) return "傳送失敗";
            var FileParh = localPath + "\\wwwroot\\PayVideo\\";
            var ImgExtension = GetRandomStringByFileName(10) + ".mp4";

            if (!Directory.Exists(FileParh))
            {
                Directory.CreateDirectory(FileParh);
            }
            using (var fileStream = new FileStream(Path.Combine(FileParh + ImgExtension), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            return "/PayVideo/" + ImgExtension;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 34359738368)]
        public string changeFileVideo(IFormFile file , string Id)
        {
            if (file == null) return "傳送失敗";

            if (Id == null) return "傳送失敗";
            var db = new CASE20210405Context();
            List<VideoPayModel> lsModel = db.VideoPayModels.ToList();
            List<VideoPayModel> ls = lsModel.Where(c => c.Id.ToString() == Id).ToList();
            if (ls.Count == 0) return "傳送失敗";
            VideoPayModel it = ls.FirstOrDefault();


            var FileParh = localPath + "\\wwwroot\\PayVideo\\";
            var ImgExtension = GetRandomStringByFileName(10) + ".mp4";

            it.videoUrl = "/PayVideo/" + ImgExtension;
            if (ModelState.IsValid)
            {
                db.Entry(it).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (!Directory.Exists(FileParh))
            {
                Directory.CreateDirectory(FileParh);
            }
            using (var fileStream = new FileStream(Path.Combine(FileParh + ImgExtension), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            return "/PayVideo/" + ImgExtension;
        }

        [HttpPost]
        public string deleteFileCallBack(string file, string newfile, string path)
        {
            var videoParh = localPath + "\\wwwroot\\" + path + "\\";
            var filename = videoParh + Path.GetFileName(file);

            if (file != null)
            {
                if (System.IO.File.Exists(filename))
                {
                    try
                    {
                        System.IO.File.Delete(filename);
                    }
                    catch (Exception) { };
                }

            }
            return newfile;
        }



        [HttpPost]
        public string ClearFile()
        {
            var videoParh = localPath + "\\wwwroot\\PayVideo\\";
            deleteFileFunc(videoParh, "PayVideo");
            var ImagePath = localPath + "\\wwwroot\\PayVideoImage\\";
            deleteFileFunc(ImagePath, "PayVideoImage");
            return "整理完成";
        }

        public void deleteFileFunc(string path,string pathData)
        {
            var db = new CASE20210405Context();
            var lsModel = db.VideoPayModels.ToList();
            var files = Directory.GetFiles(path).ToList();

            foreach (var it in lsModel)
            {
                string fileName = "";
                if (pathData.Equals("PayVideo"))
                {
                    fileName = Path.GetFileName(it.videoUrl);
                }
                else
                {
                    fileName = Path.GetFileName(it.videoImageUrl);
                }
                for(int i = 0; i < files.Count; i++)
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


        public IActionResult Index(string search = "", string searchSelect = "1", int page = 1, bool FuzzyOrNormal = false)
        {
            if (HttpContext.User.Claims.Count() == 0) return RedirectToAction("Index", "Login");
            var db = new CASE20210405Context();
            List<VideoPayModel> lsModelout = db.VideoPayModels.ToList();
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
            List<VideoPayModel> lsModelout = JsonConvert.DeserializeObject<List<VideoPayModel>>(strlsModel);

            if (search == null) search = "";
            List<VideoPayModel> serachModelout;
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.videoTitle.Equals(search)).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.videoIntro.Equals(search)).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.videoTitle.Equals(search)).ToList();
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
            List<VideoPayModel> lsModelout = JsonConvert.DeserializeObject<List<VideoPayModel>>(strlsModel);

            if (search == null) search = "";
            List<VideoPayModel> serachModelout = new List<VideoPayModel>();
            switch (searchSelect)
            {
                case "1":
                    serachModelout = lsModelout.Where(c => c.videoTitle.IndexOf(search) > -1).ToList();
                    break;
                case "2":
                    serachModelout = lsModelout.Where(c => c.videoIntro.IndexOf(search) > -1).ToList();
                    break;
                default:
                    serachModelout = lsModelout.Where(c => c.videoTitle.IndexOf(search) > -1).ToList();
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
        public RedirectToActionResult Create(VideoPayModel it)
        {
            if (string.IsNullOrEmpty(it.videoTitle))
                return RedirectToAction("Index");
            var db = new CASE20210405Context();
            db.VideoPayModels.Add(it);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="path"></param>
        public void deleteFile(string Url,string path)
        {
            var fileName = Path.GetFileName(Url);
            var pathFile = localPath + "\\wwwroot\\"+ path +"\\"+ fileName;
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
            VideoPayModel it = db.VideoPayModels.Find(Id);
            db.VideoPayModels.Remove(it);
            db.SaveChanges();
            deleteFile(it.videoImageUrl, "PayVideoImage");
            deleteFile(it.videoUrl, "PayVideo");            
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid? Id)
        {
            if (Id == null) return RedirectToAction("Index");
            var db = new CASE20210405Context();

            List<VideoPayModel> lsModel = db.VideoPayModels.ToList();
            List<VideoPayModel> ls = lsModel.Where(c => c.Id == Id).ToList();
            if (ls.Count == 0) return RedirectToAction("Index");
            VideoPayModel it = ls.FirstOrDefault();
            return View(it);
        }

        [HttpPost]
        public RedirectToActionResult Edit(VideoPayModel it)
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
