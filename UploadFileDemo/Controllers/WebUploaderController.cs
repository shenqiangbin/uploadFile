﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadFileDemo.Controllers
{
    public class WebUploaderController : Controller
    {
        // GET: WebUploader
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckChunk()
        {
            return Content("");
        }

        public ActionResult MergeChunks()
        {
            return Content("");
        }

        public ActionResult UploadFile()
        {
            return Content("");
        }
    }
}