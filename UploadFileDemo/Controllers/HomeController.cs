using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadFileDemo.Lib;
using UploadFileDemo.Models;

namespace UploadFileDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(TestModel model, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.Msg = "没有文件";
                return View();
            }
            else if (!CheckFileType(file))
            {
                ViewBag.Msg = "文件类型不对";
                return View();
            }
            else if (!CheckFileSize(file))
            {
                ViewBag.Msg = "文件大于请小于20M";
                return View();
            }

            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
            try
            {
                file.SaveAs(fileName);
                model.AttachmentPath = "../upload/" + Path.GetFileName(file.FileName);
                ViewBag.Msg = "上传成功";
            }
            catch (Exception ex)
            {
                ViewBag.Msg = ex.Message;
            }

            return View();
        }

        private bool CheckFileType(HttpPostedFileBase file)
        {
            string extension = file.FileName.Substring(file.FileName.LastIndexOf("."));
            List<string> extens = new List<string>() { ".pdf", ".doc" };
            return extens.Contains(extension);
        }

        private bool CheckFileSize(HttpPostedFileBase file)
        {
            var size = file.ContentLength / 1024 / 1024;
            return size > 20; //如果大于20M
        }

        public ActionResult UploadifyUpload()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Handle(HttpPostedFileBase fileData)
        {
            string fileName = Path.GetFileName(fileData.FileName);
            var filePath = Path.Combine(Request.MapPath("~/Upload"), fileName);
            fileData.SaveAs(filePath);
            var thumbnailFile = Path.Combine(Request.MapPath("~/Upload/Thumbnail"), fileName);
            ImgHelper.GetPicThumbnail(filePath, thumbnailFile, 133, 200, 100);
            return Json(new { Success = true, FileName = "/Upload/" + fileName, ThumbnailFile = "/Upload/Thumbnail/" + fileName });
        }        
    }
}