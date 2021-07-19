using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Sys;
using Models.SysFunc;
using Service;
using Service.CommonHelper;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// 获取格式化菜单
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost("GetMenuListInitChildren")]
        public ResponseResult<List<DIC_Menu>> GetMenuListInitChildren()
        {
            ResponseResult<List<DIC_Menu>> res = new ResponseResult<List<DIC_Menu>>();
            try
            {
                res.Entity = SysFuncService.Instance.GetMenuListInitChildren();
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
        /// 获取格式化菜单
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpGet("GetUserPromissionIDs")]
        public ResponseResult<List<Guid>> GetUserPromissionIDs(Guid userID)
        {
            ResponseResult<List<Guid>> res = new ResponseResult<List<Guid>>();
            try
            {
                res.Entity = SysFuncService.Instance.GetUserPromissionIDs(userID);
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

        [Authorize]
        [HttpGet("DownloadFile")]
        public ResponseResult<FileContentResult> DownloadFile(string filePath)
        {
            filePath = Encrypt.DESDecrypt(filePath);
            ResponseResult<FileContentResult> res = new ResponseResult<FileContentResult>();
            try
            {
                using var sw = new FileStream(filePath, FileMode.Open);
                var contenttype = FileHelper.GetContentTypeForFileName(filePath);
                var bytes = new byte[sw.Length];
                sw.Read(bytes, 0, bytes.Length);
                sw.Close();
                res.Status = ResponseResultStatus.Success;
                res.Entity = new FileContentResult(bytes, contenttype);
            }
            catch (Exception e)
            {
                res.Status = ResponseResultStatus.Error;
                res.Message = $"文件加载失败,{e.Message}";
            }
            return res;
        }
    }
}
