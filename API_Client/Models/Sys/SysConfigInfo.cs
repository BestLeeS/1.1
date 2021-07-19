using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Sys
{
    public class SysConfigInfo
    {
        public static string SecurityKey { get; set; }
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static string DBConnectionString { get; set; }
        public static string UploadFilePath { get; set; }
        public static string UploadVoiceFilepath { get; set; }
        public static string APIaddress { get; set; }
        public static string WebSocketAddress { get; set; }
    }
}
