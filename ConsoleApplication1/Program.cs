using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadFileDemo.Lib;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ImgHelper.GetPicThumbnail(@"D:\1.jpg", @"D:\1.1png", 133, 200, 100);
        }
    }
}
