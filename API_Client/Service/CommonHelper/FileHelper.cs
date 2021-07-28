using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Service.CommonHelper
{
    public class FileHelper
    {
        public static string SaveFile(IFormFile file, string folderPath, string fileName, string fileType)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            using (FileStream fs = System.IO.File.Create(folderPath + fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            return folderPath + fileName;
        }

        /// <summary>
        /// 文件转换成Base64字符串
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns></returns>
        public static String FileToBase64(string fileName)
        {
            string strRet = null;

            FileStream fs = new FileStream(fileName, FileMode.Open);
            byte[] bt = new byte[fs.Length];
            fs.Read(bt, 0, bt.Length);
            strRet = Convert.ToBase64String(bt);
            fs.Close();

            return strRet;
        }

        /// <summary>
        /// Base64字符串转换成文件
        /// </summary>
        /// <param name="strInput">base64字符串</param>
        /// <param name="fileName">保存文件的绝对路径</param>
        /// <returns></returns>
        public static bool Base64ToFileAndSave(string strInput, string fileName)
        {
            bool bTrue = false;
            byte[] buffer = Convert.FromBase64String(strInput);
            FileStream fs = new FileStream(fileName, FileMode.CreateNew);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
            bTrue = true;
            return bTrue;
        }

        public static string GetContentTypeForFileName(string fileName)
        {
            //获取文件后缀
            string ext = Path.GetExtension(fileName);
            using (Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext))
            {
                if (registryKey == null)
                    return null;
                var value = registryKey.GetValue("Content Type");
                return value?.ToString();
            }
        }
    }
}
