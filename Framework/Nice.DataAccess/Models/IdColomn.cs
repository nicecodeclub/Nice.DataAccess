using Nice.DataAccess.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nice.DataAccess.Models
{
    public class IdColomnInfo
    {
        private readonly IList<IdColomn> ids = new List<IdColomn>();

        private IdColomn firstIdColomn;
        public void Add(PropertyInfo property, string IdColomnName, IdAttribute idinfo)
        {
            IdColomn idColomn = new IdColomn();
            idColomn.IdProperty = property;
            idColomn.ColomnName = IdColomnName;
            idColomn.GenerateType = idinfo.GenerateType;
            if (firstIdColomn == null)
                firstIdColomn = idColomn;
            ids.Add(idColomn);
        }

        public int GetCount()
        {
            return ids.Count;
        }

        public string GetFirstColomnName()
        {
            return firstIdColomn.ColomnName;
        }
        public IdGenerateType GetIdGenerateType()
        {
            return firstIdColomn.GenerateType;
        }

        public bool EqualName(PropertyInfo property)
        {
            IdColomn idColomn = null;
            for (int i = 0; i < ids.Count; i++)
            {
                idColomn = ids[i];
                if (idColomn.IdProperty.Name == property.Name)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
    public class IdColomn
    {
        private string colomnName;
        /// <summary>
        /// 主键列名
        /// </summary>
        public string ColomnName
        {
            get { return colomnName; }
            set { colomnName = value; }
        }
        private PropertyInfo idProperty;
        /// <summary>
        /// 主键属性
        /// </summary>
        public PropertyInfo IdProperty
        {
            get { return idProperty; }
            set { idProperty = value; }
        }
        private IdGenerateType generateType;
        /// <summary>
        /// ID生成类型
        /// </summary>
        public IdGenerateType GenerateType
        {
            get { return generateType; }
            set { generateType = value; }
        }
    }
}
