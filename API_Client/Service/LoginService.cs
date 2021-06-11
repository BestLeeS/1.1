using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Sys;
using Service.CommonHelper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class LoginService
    {
        public static LoginService _instance;
        public static LoginService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoginService();
                }
                return _instance;
            }

        }

        public Sys_User GetToken(string loginName,String pwd)
        {
            string enCryptedPwd = Encrypt.DESEncrypt(pwd);
            List<DIC_Menu> resMenus = new List<DIC_Menu>();
            Sys_User sys_User = SugarHelper.DB.Queryable<Sys_User>().Where(x=>x.LoginName == loginName && x.PassWord == enCryptedPwd).First();
            if (sys_User != null)
            {
                var userMenus = SugarHelper.DB.Queryable<User_Promission, DIC_Menu>((t1, t2) => new JoinQueryInfos(JoinType.Left, t1.MenuID == t2.id))
                    .Where((t1,t2) => t1.UserID == sys_User.UserID && t2.id != Guid.Empty && t2.Enable == true)
                    .Select<DIC_Menu>().ToList();
                if (userMenus.Count == 0)
                    throw new Exception("当前用户无操作权限,请联系管理员!");
                else 
                {
                    resMenus = userMenus.FindAll(z => z.parentID == Guid.Empty).OrderBy(z=>z.orderNum).ToList();
                    List<DIC_Menu> childMenus = userMenus.FindAll(z => z.parentID != Guid.Empty).OrderBy(z => z.orderNum).ToList();
                    childMenus.ForEach(x => {
                        var tmpResMenus = resMenus.Find(z => z.id == x.parentID);
                        x.meta.title = x.title;
                        x.meta.icon = x.icon;
                        if (tmpResMenus != null)
                            x.parentName = tmpResMenus.name;
                    });
                    resMenus.ForEach(x => {
                        x.meta.title = x.title;
                        x.meta.icon = x.icon;
                        x.children = childMenus.FindAll(z => z.parentID == x.id);
                    });

                }
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,loginName),
                    new Claim(ClaimTypes.NameIdentifier,pwd),
                };

                var tmpToken = new JwtSecurityToken(
                            issuer: SysConfigInfo.Issuer,
                            audience: SysConfigInfo.Audience,
                            claims: claims,
                            notBefore: DateTime.Now,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SysConfigInfo.SecurityKey)), SecurityAlgorithms.HmacSha256));

                //生成Token
                string jwtToken = new JwtSecurityTokenHandler().WriteToken(tmpToken);
                return new Sys_User() { 
                    UserID = sys_User.UserID,
                    LoginName = sys_User.LoginName,
                    UserName = sys_User.UserName,
                    RoleCode = sys_User.RoleCode,
                    Token = jwtToken,
                    Menus = resMenus
                };
            }
            else
                throw new Exception("用户不存在,或用户名或密码错误,请重试!");
        }
    }
}
