using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Sys
{
    public class TokenObj
    {
        public string Token { get; set; }//token内容
        public long Expires { get; set; }//过期时间
    }
}
