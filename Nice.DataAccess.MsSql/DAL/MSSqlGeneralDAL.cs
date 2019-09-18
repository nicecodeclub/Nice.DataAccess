using Nice.DataAccess.DAL;
using Nice.DataAccess.Models;
using System.Text;

namespace Nice.DataAccess.MsSql.DAL
{
    /// <summary>
    /// 数据访问层基础类
    /// </summary>
    /// <typeparam name="T">TEntity实体</typeparam>
    public class MsSqlGeneralDAL<T> : BaseDAL<T>, IGeneralDAL<T> where T : TEntity, new()
    {
        public MsSqlGeneralDAL(string connStrKey) : base(connStrKey) { }

        protected override string GetLastIncrementID()
        {
            return "SELECT SCOPE_IDENTITY()";
        }

        protected override string GetInsertOrUpdateSql()
        {
            string IdParameterName = string.Format("{0}{1}", DataHelper.GetParameterPrefix(), IdColomn.IdProperty.Name);
            StringBuilder sb = new StringBuilder();
            sb.Append(" begin tran");
            sb.AppendFormat(" if exists (select {0} from {1} where {0}={2})", IdColomn.ColomnName, TableName, IdParameterName);
            sb.Append(" begin");
            sb.AppendFormat(" update {0} set {1}", TableName, SetColumnText);
            sb.AppendFormat(" where {0}={1}", IdColomn.ColomnName, IdParameterName);
            sb.Append(" end else begin");
            sb.AppendFormat(" insert into {0}({1})", TableName, InsertColumnText);
            sb.AppendFormat(" values({0}) end", InsertColumnValue);
            sb.Append(" commit tran");
            return sb.ToString();
        }
    }
}
