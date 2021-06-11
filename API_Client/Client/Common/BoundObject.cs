using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Client
{
    public class BoundObject
    {
        public string IDCardNum { get; set; } = string.Empty;
        public event EventHandler OnGetIdCard;
        public event EventHandler OnShot;
        public event PrintEventHandler OnPrint;
        public event SaveEventHandler OnSave;
        private IJavascriptCallback JsGetIdcardCallback = null;
        private IJavascriptCallback JsShotCallback = null;
        public BoundObject()
        {
        }
        #region js接口函数

        /// <summary>
        /// 刷身份证
        /// </summary>
        public int getIdcard(IJavascriptCallback javascriptCallback)
        {
            JsGetIdcardCallback = javascriptCallback;
            OnGetIdCard?.Invoke(this, new System.EventArgs());
            return 0;
        }
        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="javascriptCallback"></param>
        /// <returns></returns>
        public int Shot(IJavascriptCallback javascriptCallback)
        {
            JsShotCallback = javascriptCallback;
            OnShot?.Invoke(this, new System.EventArgs());
            return 0;
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="base64String">PDF文件Base64String编码</param>
        public int Print(string base64String)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = base64String, flag = 0 });
            return 1;
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="base64String">
        /// Bmp文件Base64String编码
        /// </param>
        /// <param name="pageCount">
        /// 打印数量
        /// </param>
        /// <returns></returns>

        public int PrintBarCode(string base64String, int pageCount)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = base64String, flag = 1 });
            return 0;
        }
        //打印条码
        public int PrintBill(string base64String)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = base64String, flag = 2 });
            return 0;
        }
        //打印姓名条码
        public int PrintNameCode(string OderCodes)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = OderCodes, flag = 3 });
            return 0;
        }
        //打印导检单
        public int PrintGuideData(string OderCodes)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = OderCodes, flag = 4 });
            return 0;
        }
        //打印体检报告
        public int PrintRpts(string ReportCodes)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = ReportCodes, flag = 5 });
            return 0;
        }
        //客户端打印条码
        public int PrintBarCodeByClient(string BarCodeJson)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = BarCodeJson, flag = 6 });
            return 0;
        }
        //客户端打印项目名称
        public int PrintItemNames(string OderCodes)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = OderCodes, flag = 7 });
            return 0;
        }
        //客户端打印Lis检查结果
        public int PrintLisRePorts(string OderCodes)
        {
            OnPrint?.Invoke(this, new PrintEventArgs() { data = OderCodes, flag = 8 });
            return 0;
        }
        /// <summary>
        /// 文件保存对话框
        /// </summary>
        /// <param name="base64word"></param>
        /// <returns></returns>
        public int OpenSaveDialog(string base64word)
        {
            OnSave?.Invoke(this, new SaveEventArgs() { DataType = 1, Data = base64word });
            return 0;
        }

        public int SaveToExcel(string jsonString)
        {
            OnSave?.Invoke(this, new SaveEventArgs() { DataType = 2, Data = jsonString });
            return 0;
        }

        /// <summary>
        /// 获取系统默认打印机设置
        /// </summary>
        /// <returns></returns>
        public string GetPrinterSetting()
        {
            //var list = WinAPI.GetPrinerList();
            //string json = JsonConvert.SerializeObject(list);
            //return json;
            var printerSetting = WinAPI.GetPrinerSetting();
            string json = JsonConvert.SerializeObject(printerSetting);
            return json;

        }
        /// <summary>
        /// 设置默认打印机
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        public int SavePrinterSetting(string defaultDocPrinter, string defaultBarcodePrinter, string defaultBillPrinter)
        {
            WinAPI.SetDefaultPrinter(1, defaultDocPrinter);
            WinAPI.SetDefaultPrinter(2, defaultBarcodePrinter);
            WinAPI.SetDefaultPrinter(3, defaultBillPrinter);
            return 0;
        }
        #endregion
        #region API回调函数
        /// <summary>
        /// 读身份证回调函数
        /// </summary>
        /// <param name="data"></param>
        public void GetIdcardCallBack(object data)
        {
            if (JsGetIdcardCallback != null)
            {
                //JsGetIdcardCallback.ExecuteAsync(data);
                JsGetIdcardCallback.ExecuteAsync(JsonConvert.SerializeObject(data));
                JsGetIdcardCallback = null;
            }
        }
        /// <summary>
        /// 拍照回调函数
        /// </summary>
        /// <param name="data"></param>
        public void ShotCallBack(object data)
        {
            if (JsShotCallback != null)
            {
                JsShotCallback.ExecuteAsync(data);
                JsShotCallback = null;
            }
        }
        #endregion

    }
}
