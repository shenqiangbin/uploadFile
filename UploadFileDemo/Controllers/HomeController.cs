using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                ViewBag.Msg = "没有文件";

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
    }
}