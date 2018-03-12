using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadFileDemo.Controllers
{
    /// <summary>
    /// 参考地址：https://www.cnblogs.com/war-hzl/p/7560083.html
    /// 测试地址：http://192.168.105.78/uploadfiledemo/WebUploader2
    /// </summary>
    public class WebUploader2Controller : Controller
    {
        // GET: WebUploader
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadChunk()
        {
            //如果进行了分片
            if (Request.Form.AllKeys.Any(m => m == "chunk"))
            {
                //取得chunk和chunks
                int chunk = Convert.ToInt32(Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                int chunks = Convert.ToInt32(Request.Form["chunks"]);//总分片数
                //根据GUID创建用该GUID命名的临时文件夹
                string folder = Server.MapPath("~/upload/" + Request["guid"] + "/");
                string path = folder + chunk;

                //建立临时传输文件夹
                if (!Directory.Exists(Path.GetDirectoryName(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                FileStream addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                BinaryWriter AddWriter = new BinaryWriter(addFile);
                //获得上传的分片数据流
                var file = Request.Files[0];
                Stream stream = file.InputStream;

                BinaryReader TempReader = new BinaryReader(stream);
                //将上传的分片追加到临时文件末尾
                AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                //关闭BinaryReader文件阅读器
                TempReader.Close();
                stream.Close();
                AddWriter.Close();
                addFile.Close();

                TempReader.Dispose();
                stream.Dispose();
                AddWriter.Dispose();
                addFile.Dispose();
                return Json(new { chunked = true, hasError = false, f_ext = Path.GetExtension(file.FileName) });
            }
            else//没有分片直接保存
            {
                Request.Files[0].SaveAs(Server.MapPath("~/upload/" + DateTime.Now.ToFileTime() + Path.GetExtension(Request.Files[0].FileName)));
                return Json(new { chunked = true, hasError = false });
            }
        }

        public ActionResult MergeChunks()
        {
            try
            {
                var guid = Request["guid"];//GUID
                var uploadDir = Server.MapPath("~/upload");//Upload 文件夹
                var dir = Path.Combine(uploadDir, guid);//临时文件夹
                if (Directory.Exists(dir))
                {
                    var ext = Path.GetExtension(Request["fileName"]);
                    var files = Directory.GetFiles(dir);//获得下面的所有文件
                    var name = Guid.NewGuid().ToString("N") + ext;
                    var finalPath = Path.Combine(uploadDir, name);//最终的文件名
                    var fs = new FileStream(finalPath, FileMode.Create);
                    foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
                    {
                        var bytes = System.IO.File.ReadAllBytes(part);
                        fs.Write(bytes, 0, bytes.Length);
                        bytes = null;
                        System.IO.File.Delete(part);//删除分块
                    }
                    fs.Flush();
                    fs.Close();
                    Directory.Delete(dir);//删除文件夹
                    return Json(new { r = 1, path = "/upload/" + name });
                }
                else
                {
                    return Json(new { r = 1, path = "" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { r = 0, err = ex.Message });
            }
        }

        //检查当前分块是否上传成功 
        public ActionResult CheckChunk(string fileMd5, string chunk, string chunkSize)
        {
            string folder = Server.MapPath("~/upload/" + fileMd5 + "/");
            string path = folder + chunk;
            FileInfo checkFile = new FileInfo(folder + chunk);

            //检查文件是否存在，且大小是否一致  
            if (checkFile.Exists && checkFile.Length == int.Parse(chunkSize))
            {
                return Json(new { ifExist = 1 });
            }
            else
            {
                //没有上传过  
                return Json(new { ifExist = 0 });
            }
        }
    }
}