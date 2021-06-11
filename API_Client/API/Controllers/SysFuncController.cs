using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Sys;
using Models.SysFunc;
using Service;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysFuncController : Controller
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost("GetMenuList")]
        [Authorize]

        public ResponseResult<PaginatedList<DIC_Menu>> GetMenuList(SearchParams searchParams)
        {
            ResponseResult<PaginatedList<DIC_Menu>> res = new ResponseResult<PaginatedList<DIC_Menu>>();
            try
            {
                res.Entity = SysFuncService.Instance.GetMenuList(searchParams);
                res.Status = ResponseResultStatus.Success;
            }
            catch (Exception e)
            {
                res.Status = ResponseResultStatus.Error;
                res.Message = e.Message;
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost("GetMenuByInnerCode")]
        [Authorize]

        public ResponseResult<DIC_Menu> GetMenuByInnerCode(ReqSysFunc reqSysFunc)
        {
            ResponseResult<DIC_Menu> res = new ResponseResult<DIC_Menu>();
            try
            {
                res.Entity = SysFuncService.Instance.GetMenuByInnerCode(reqSysFunc);
                res.Status = ResponseResultStatus.Success;
            }
            catch (Exception e)
            {
                res.Status = ResponseResultStatus.Error;
                res.Message = e.Message;
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost("InsertOrUpdateMenu")]
        [Authorize]

        public ResponseResult InsertOrUpdateMenu(ReqDICMenu dIC_Menu)
        {
            ResponseResult res = new ResponseResult();
            try
            {
                SysFuncService.Instance.InsertOrUpdateMenu(dIC_Menu);
                res.Status = ResponseResultStatus.Success;
            }
            catch (Exception e)
            {
                res.Status = ResponseResultStatus.Error;
                res.Message = e.Message;
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost("DelMenus")]
        [Authorize]

        public ResponseResult DelMenus(List<Guid> innerCodes)
        {
            ResponseResult res = new ResponseResult();
            try
            {
                SysFuncService.Instance.DelMenus(innerCodes);
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
