using System.Data;

namespace Nice.DataAccess.Models
{
    public class FilterValid
    {
        private IDataParameter paramFilterValid;
        public IDataParameter ParamFilterValid
        {
            get { return paramFilterValid; }
            set { paramFilterValid = value; }
        }
        private IDataParameter paramFilterInValid;
        public IDataParameter ParamFilterInValid
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
        private string propertyName;
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }
    }
}
