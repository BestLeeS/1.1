using Client.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;

namespace Client
{
    public class WinAPI
    {
        public class PrinterSetting
        {
            public string value { get; set; }
            public List<Key_Value> options { get; set; }
        }
        public BoundObject boundObject = null;

        public WinAPI()
        {
            boundObject = new BoundObject();
            #region 拍照
            boundObject.OnShot += (sender, e) =>
            {
                Image image = Cam.Camera.Shot();
                if (image != null)
                {
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    byte[] bytes = ms.GetBuffer(); 
                    ms.Close();
                    var result = Convert.ToBase64String(bytes);
                    boundObject.ShotCallBack(result);
                }
            };
            #endregion

        }

        public void Base642File(string base64string, string fileName)
        {
            try
            {
                byte[] bt = Convert.FromBase64String(base64string);
                FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                fs.Write(bt, 0, bt.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        public static List<string> GetPrinerList()
        {
            List<string> pList = new List<string>();
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                pList.Add(sPrint);
            }
            return pList;
        }

        public static List<PrinterSetting> GetPrinerSetting()
        {
            List<Key_Value> pList = new List<Key_Value>();
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                pList.Add(new Key_Value() { Key = sPrint, Value = sPrint });
            }
            List<PrinterSetting> printerSettings = new List<PrinterSetting>();
            for (int i = 1; i <= 3; i++)
            {
                var p = GetDefaultPrinter(i);
                printerSettings.Add(new PrinterSetting() { value = p, options = pList });
            }
            return printerSettings;
        }

        public static string GetDefaultPrinter(int printType)
        {
            string defaultPrinter = string.Empty;
            switch (printType)
            {
                case 1:
                    {
                        defaultPrinter = ConfigurationManager.AppSettings["DefaultDocPrinter"];
                    }
                    break;
                case 2:
                    {
                        
                    }
                    break;
                case 3:
                    {
                        
                    }
                    break;
            }
            return defaultPrinter;
        }
        public static void SetDefaultPrinter(int printType, string defaultPrinter)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            switch (printType)
            {
                case 1:
                    {
                        config.AppSettings.Settings["DefaultDocPrinter"].Value = defaultPrinter;
                    }
                    break;
                case 2:
                    {
                        
                    }
                    break;
                case 3:
                    {
                        
                    }
                    break;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

        }
    }
}
