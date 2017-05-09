using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace UploadFileDemo.Lib
{
    public class ImgHelper
    {
        /// <summary>
        /// 生成缩略图重载方法1，返回缩略图的Image对象
        /// </summary>
        /// <param name="Width">缩略图的宽度</param>
        /// <param name="Height">缩略图的高度</param>
        /// <returns>缩略图的Image对象</returns>
        public static Image GetReducedImage(string ImageFilePathAndName, int Width, int Height)
        {
            Image ResourceImage = Image.FromFile(ImageFilePathAndName);

            //用指定的大小和格式初始化Bitmap类的新实例
            Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            //从指定的Image对象创建新Graphics对象
            Graphics graphics = Graphics.FromImage(bitmap);
            //清除整个绘图面并以透明背景色填充
            graphics.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片对象
            graphics.DrawImage(ResourceImage, new Rectangle(0, 0, Width, Height));
            return bitmap;
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();

            }
        }

        /// <summary>
        /// 截取图片方法
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="beginX">开始位置-X</param>
        /// <param name="beginY">开始位置-Y</param>
        /// <param name="getX">截取宽度</param>
        /// <param name="getY">截取长度</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="fileExt">后缀名</param>
        public static string CutImage(string url, int beginX, int beginY, int getX, int getY, string fileName, string savePath, string fileExt)
        {
            if ((beginX < getX) && (beginY < getY))
            {
                Bitmap bitmap = new Bitmap(url);//原图
                if (((beginX + getX) <= bitmap.Width) && ((beginY + getY) <= bitmap.Height))
                {
                    Bitmap destBitmap = new Bitmap(getX, getY);//目标图
                    Rectangle destRect = new Rectangle(0, 0, getX, getY);//矩形容器
                    Rectangle srcRect = new Rectangle(beginX, beginY, getX, getY);

                    var graphics = Graphics.FromImage(destBitmap);
                    graphics.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);

                    ImageFormat format = ImageFormat.Png;
                    switch (fileExt.ToLower())
                    {
                        case "png":
                            format = ImageFormat.Png;
                            break;
                        case "bmp":
                            format = ImageFormat.Bmp;
                            break;
                        case "gif":
                            format = ImageFormat.Gif;
                            break;
                    }
                    destBitmap.Save(savePath + "//" + fileName, format);
                    return savePath + "\\" + "*" + fileName.Split('.')[0] + "." + fileExt;
                }
                else
                {
                    return "截取范围超出图片范围";
                }
            }
            else
            {
                return "请确认(beginX < getX)&&(beginY < getY)";
            }
        }
    }
}

