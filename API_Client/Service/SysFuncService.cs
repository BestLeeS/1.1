using Models;
using Models.Sys;
using Models.SysFunc;
using Service.CommonHelper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class SysFuncService
    {
        public static SysFuncService _instance;
        public static SysFuncService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SysFuncService();
                }
                return _instance;
            }
        }

        public PaginatedList<DIC_Menu> GetMenuList(SearchParams searchParams) 
        {
            int totalCount = 0;
            var tmpList = SugarHelper.DB.Queryable<DIC_Menu, DIC_Menu>((t1, t2) => new JoinQueryInfos(JoinType.Left, t1.parentID == t2.id));
            if (!string.IsNullOrEmpty(searchParams.QueryName))
                tmpList.Where(t1 => searchParams.QueryName.Contains(t1.title) || t1.title.Contains(searchParams.QueryName));
            var list = tmpList.Select((t1,t2)=> new DIC_Menu { 
                id = t1.id,
                index = t1.index,
                name = t1.name,
                path = t1.path,
                componentpath = t1.componentpath,
                redirect = t1.redirect,
                parentID = t1.parentID,
                parentName = t2.title,
                title = t1.title,
                icon = t1.icon,
                orderNum = t1.orderNum,
                Enable = t1.Enable
            }).OrderBy("t1.parentID,t1.orderNum asc").ToPageList(searchParams.PageIndex,searchParams.PageSize,ref totalCount);
 
            PaginatedList<DIC_Menu> result = new PaginatedList<DIC_Menu>(list, totalCount, searchParams.PageIndex, searchParams.PageSize);
            return result;
            
        }

        public DIC_Menu GetMenuByInnerCode(ReqSysFunc reqSysFunc)
        {
            var queryable = SugarHelper.DB.Queryable<DIC_Menu, DIC_Menu>((t1, t2) => new JoinQueryInfos(JoinType.Left, t1.parentID == t2.id));
            var res = queryable.Select((t1, t2) => new DIC_Menu
            {
                id = t1.id,
                index = t1.index,
                name = t1.name,
                path = t1.path,
                componentpath = t1.componentpath,
                redirect = t1.redirect,
                parentID = t1.parentID,
                parentName = t2.title,
                title = t1.title,
                icon = t1.icon,
                orderNum = t1.orderNum,
                Enable = t1.Enable
            }).First(t1=>t1.id == reqSysFunc.InnerCode);

            return res;
        }

        public void InsertOrUpdateMenu(ReqDICMenu dIC_Menu)
        {
            DIC_Menu insDICMenu = new DIC_Menu
            {
                id = dIC_Menu.ID,
                title = dIC_Menu.Title,
                icon = dIC_Menu.Icon,
                index = dIC_Menu.MenuName,
                name = dIC_Menu.MenuName,
                path = string.IsNullOrEmpty(dIC_Menu.ComponentPath)|| dIC_Menu.ComponentPath == "Layout" ? "/" : dIC_Menu.MenuName,
                componentpath = string.IsNullOrEmpty(dIC_Menu.ComponentPath) ? "Layout" : dIC_Menu.ComponentPath,
                redirect = dIC_Menu.Redirect,
                parentID = dIC_Menu.ParentID.HasValue?dIC_Menu.ParentID.Value:Guid.Empty,
                orderNum = dIC_Menu.OrderNum,
                Enable = dIC_Menu.Enable
            };

            if (dIC_Menu.ID == Guid.Empty)
            {
                dIC_Menu.ID = Guid.NewGuid();
                SugarHelper.DB.Insertable(insDICMenu).ExecuteCommand();
            }
            else
                SugarHelper.DB.Updateable(insDICMenu).ExecuteCommand();
            
        }

        public void DelMenus(List<Guid> innerCodes)
        {
            SugarHelper.DB.Deleteable<DIC_Menu>().Where(z => innerCodes.Contains(z.id)).ExecuteCommand();
        }

    }
}
