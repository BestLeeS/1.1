using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Sys_User
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public string IDCardNum { get; set; }
        public string PhoneNum { get; set; }
        public string Photo { get; set; }
        public string Signature { get; set; }
        public Guid RoleCode { get; set; }
        public string Token { get; set; }
        public string Remark { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<DIC_Menu> Menus { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool IsOnline { get; set; } = false;
    }
}
