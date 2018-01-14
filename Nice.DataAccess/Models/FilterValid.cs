using System.Data;

namespace Nice.DataAccess.Models
{
    public class FilterValid
    {
        private IDbDataParameter paramFilterValid;
        public IDbDataParameter ParamFilterValid
        {
            get { return paramFilterValid; }
            set { paramFilterValid = value; }
        }
        private IDbDataParameter paramFilterInValid;
        public IDbDataParameter ParamFilterInValid
        {
            get { return paramFilterInValid; }
            set { paramFilterInValid = value; }
        }
        private string validColumnName;
        public string ValidColumnName
        {
            get { return validColumnName; }
            set { validColumnName = value; }
        }
    }
}
