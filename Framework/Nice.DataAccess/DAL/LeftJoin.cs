using Nice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Nice.DataAccess.DAL
{
    public class LeftJoin<T> where T : TEntity, new()
    {
        public IList<T> On(Expression<Func<T, bool>> expression)
        {

        }
    }
}
