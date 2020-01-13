using Nice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.DataAccess.DAL
{
    public class QueryUnionDAL<T> : QueryBaseDAL<T> where T : TEntity, new()
    {
        public QueryUnionDAL(string connStrKey) : base(connStrKey) { }

        public LeftJoin<T> LeftJoin() {

        }
    }
}
