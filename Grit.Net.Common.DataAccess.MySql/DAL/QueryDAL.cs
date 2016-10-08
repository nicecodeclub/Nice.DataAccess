using System;
using System.Collections.Generic;
using Grit.Net.Common.DAL;
using Grit.Net.Common.Models;
using Grit.Net.Common.Models.Page;

namespace Grit.Net.Common.DataAccess.DAL
{
    public partial class QueryDAL<T> : IQueryDAL<T> where T : TModel, new()
    {
        public QueryDAL()
        {
        }

        public T GetBySQL(string cmdText)
        {
            throw new NotImplementedException();
        }

        public T GetBySQL(string cmdText, object[] parmsValue)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetListBySQL(string cmdText)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetListBySQL(string cmdText, object[] parmsValue)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetListBySQL(string cmdText, PageInfo page)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetListBySQL(string cmdText, object[] parmsValue, PageInfo page)
        {
            throw new NotImplementedException();
        }
    }
}
