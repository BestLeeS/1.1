using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Sys_User
    {
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public string RoleCode { get; set; }
        public Guid UserID { get; set; }
        public string Token { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<DIC_Menu> Menus { get; set; }
    }
}
