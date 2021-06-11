using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost("GetToken")]
        public ResponseResult<Sys_User> GetToken(Sys_User loginInfo)
        {
            ResponseResult<Sys_User> res = new ResponseResult<Sys_User>();
            try
            {
                res.Entity = LoginService.Instance.GetToken(loginInfo.LoginName, loginInfo.PassWord);
                res.Status = ResponseResultStatus.Success;
            }
            catch(Exception e)
            {
                res.Status = ResponseResultStatus.Error;
                res.Message = e.Message;
            }
            return res;
        }

        [HttpPost("TokenTest")]
        [Authorize]
        public ResponseResult<Sys_User> TokenTest(Sys_User loginInfo)
        {
            ResponseResult<Sys_User> res = new ResponseResult<Sys_User>();
            try
            {
                res.Entity = LoginService.Instance.GetToken(loginInfo.LoginName, loginInfo.PassWord);
                res.Status = ResponseResultStatus.Success;
            }
            catch (Exception e)
            {
                res.Status = ResponseResultStatus.Error;
                res.Message = e.Message;
            }
            return res;
        }
    }
}
