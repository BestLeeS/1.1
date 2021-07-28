using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Service.CommonHelper
{
    /// <summary>
    /// 图像转换类
    /// </summary>
    public class ImageHelper
    {
        public static string ImageFile2Bases64Code(string fileName, ImageFormat imageFormat)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName)) return string.Empty;
                Bitmap bitmap = new Bitmap(fileName);
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, imageFormat);
                byte[] bytes = ms.GetBuffer();
                bitmap.Dispose();
                ms.Close();
                return Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
        }
        public static string ImgToBase64(string ImageFileName)
        {
            try
            {
                Bitmap bmp = new Bitmap(ImageFileName);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static Bitmap GetImageFromBase64(string base64Code)
        {
            try
            {
                byte[] b = Convert.FromBase64String(base64Code);
                MemoryStream ms = new MemoryStream(b);
                Bitmap bitmap = new Bitmap(ms);
                return bitmap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static byte[] Bitmap2Bytes(Bitmap bitmap, ImageFormat imageFormat)
        {
            try
            {
                if (bitmap == null)
                {
                    return null;
                }
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, imageFormat);
                byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释
                ms.Close();
                return bytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Bitmap Bytes2Bitmap(byte[] bytes)
        {
            try
            {
                byte[] bytelist = bytes;
                MemoryStream ms = new MemoryStream(bytelist);
                Bitmap bm = (Bitmap)Image.FromStream(ms);
                ms.Close();
                return bm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        public static string Bitmap2Bases64Code(Bitmap bitmap, ImageFormat imageFormat)
        {
            try
            {
                if (bitmap == null)
                {
                    return null;
                }
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, imageFormat);
                byte[] bytes = ms.GetBuffer();
                ms.Close();
                return Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
             
        public static void  Bases64Code2ImageFile(string base64Code, string fileName)
        {
            try
            {
                List<string> base64CodeArry = base64Code.Split(",").ToList();
                Bitmap bitmap = GetImageFromBase64(base64CodeArry[1]);
                bitmap.Save(fileName, GetFormat(base64CodeArry[0].Split(";")[0].Split("/")[1]));
                
            }
            catch (Exception e)
            {
                throw new Exception("图片保存失败," + e.Message);       
            }
        }

        public static void Bases64Code2ImageFileBybyte(string base64Code, string fileName)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64Code);
                using (FileStream fstrm = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter writer = new BinaryWriter(fstrm))
                    {
                        writer.Write(bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ImageFormat GetFormat(string forMatStr)
        {
            ImageFormat imageFormat = null;
            switch (forMatStr.ToUpper())
            {
                case "JPG":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case "JPEG":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case "PNG":
                    imageFormat = ImageFormat.Png;
                    break;
                case "GIF":
                    imageFormat = ImageFormat.Gif;
                    break;
                case "BMP":
                    imageFormat = ImageFormat.Bmp;
                    break;
                default:
                    imageFormat = ImageFormat.Jpeg;
                    break;
            }
            return imageFormat;

        }
    }

}
