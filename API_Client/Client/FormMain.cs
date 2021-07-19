using AutoUpdaterDotNET;
using CefSharp;
using CefSharp.WinForms;
using Client.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class FormMain : Form
    {

        private ChromiumWebBrowser webBrowser = null;   // 浏览器对象
        WinAPI winAPI = null;     // 浏览器客户端调用WinForm代码块对象
        private string Root = ConfigurationManager.AppSettings["Root"];
        private string DeBugRoot = ConfigurationManager.AppSettings["DeBugRoot"];

        public FormMain()
        {
            //删除浏览器缓存文件夹
            try
            {
                string CachePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Cache";
                if (System.IO.Directory.Exists(CachePath))
                    System.IO.Directory.Delete(CachePath, true);
            }
            catch { }

            try
            {
                #if DEBUG
                    Root = DeBugRoot;
                #else
                    //获取配置文件以外的配置参数 -- 获取自动升级的地址
                    string AutoUpdateUrl = JsonFileReadHelper.Instance.FIndValue("AutoUpdateUrl");
                    AutoUpdater.Start(AutoUpdateUrl);
                #endif
                InitializeComponent();
                this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
                InitCef();
                WindowState = FormWindowState.Maximized;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void InitCef()
        {
            // 自定义浏览器设置
            Cef.EnableHighDPISupport();
            CefSettings Settings = new CefSettings();
            Settings.Locale = "zh-CN";
            Settings.CachePath = Directory.GetCurrentDirectory() + @"\Cache";
            Settings.CefCommandLineArgs.Add("enable-media-stream", "1");  //开启摄像头
            Settings.CefCommandLineArgs.Add("enable-system-flash", "1");  //flash
            Settings.CefCommandLineArgs.Add("enable-speech-input", "1");  //语音输入
            Cef.Initialize(Settings);
            webBrowser = new ChromiumWebBrowser(Root) { Dock = DockStyle.Fill };

            // 注册客户端脚本对象
            winAPI = new WinAPI();
            webBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            
            webBrowser.JavascriptObjectRepository.Register("bound", winAPI.boundObject, isAsync: false, options: BindingOptions.DefaultBinder);
            this.Controls.Add(webBrowser);

            webBrowser.MenuHandler = new MenuHandler();

            webBrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;

            // 自定义浏览器键盘处理事件
            webBrowser.KeyboardHandler = new CEFKeyBoardHander();

            // 网页开始加载时的处理
            webBrowser.FrameLoadStart += (sender, e) =>
            {
                //执行JS代码
                //e.Frame.ExecuteJavaScriptAsync($"");
            };

            // 网页加载完毕后的处理
            webBrowser.FrameLoadEnd += (sender, e) =>
            {

            };

            webBrowser.DownloadHandler = new DownloadHandler();


        }

    }

    internal class DownloadHandler : IDownloadHandler
    {
        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
                }
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {

        }
    }

}
