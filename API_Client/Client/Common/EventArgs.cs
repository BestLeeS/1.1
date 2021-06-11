using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class PrintEventArgs: EventArgs
    {
        public string data { get; set; }
        public int flag { get; set; }
    }
    public class SaveEventArgs : EventArgs
    {
        public int DataType { get; set; }//= 1 worrd base64编码 =2 excel json字符串 
        public string Data { get; set; }
    }
}
