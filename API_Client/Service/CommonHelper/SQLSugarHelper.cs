using Models.Sys;
using SqlSugar;

namespace Service.CommonHelper
{
    public class SugarHelper
    {
        public static SqlSugarClient DB = null;

        static SugarHelper()
        {
            if (DB == null)
            {
                DB = Conn();
            }
        }

        public static SqlSugarClient Conn()
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = SysConfigInfo.DBConnectionString,//连接符字串
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            }); ;
        }
    }
}
