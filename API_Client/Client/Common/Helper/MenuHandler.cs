using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Helper
{
    public class MenuHandler : CefSharp.IContextMenuHandler
    {

        void CefSharp.IContextMenuHandler.OnBeforeContextMenu(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.IMenuModel model)
        {
            model.Clear();
            model.AddItem(CefSharp.CefMenuCommand.Copy, "复制 (&C)");
            model.AddItem(CefSharp.CefMenuCommand.Cut, "剪切 (&X)");
            model.AddItem(CefSharp.CefMenuCommand.Paste, "粘贴 (&P)");
            model.AddItem(CefSharp.CefMenuCommand.Delete, "清除 (&D)");
        }

        bool CefSharp.IContextMenuHandler.OnContextMenuCommand(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.CefMenuCommand commandId, CefSharp.CefEventFlags eventFlags)
        {
            //throw new NotImplementedException();
            return false;
        }

        void CefSharp.IContextMenuHandler.OnContextMenuDismissed(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame)
        {
            //throw new NotImplementedException();
        }

        bool CefSharp.IContextMenuHandler.RunContextMenu(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.IMenuModel model, CefSharp.IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
