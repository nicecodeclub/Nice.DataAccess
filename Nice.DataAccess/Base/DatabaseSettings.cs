using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.DataAccess
{
    public class DatabaseSettings
    {
        private string connString;
        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }
        private string providerName;
        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }
        private int commandTimeout;
        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }
        private string dataProviderAssembly;
        public string DataProviderAssembly
        {
            get
            {
                return dataProviderAssembly;
            }
            set
            {
                dataProviderAssembly = value;
            }
        }
        private string dataProviderTypeName;
        public string DataProviderTypeName
        {
            get
            {
                return dataProviderTypeName;
            }
            set
            {
                dataProviderTypeName = value;
            }
        }

        private string dataFactoryGeneralDAL;
        public string DataFactoryGeneralDAL
        {
            get
            {
                return dataFactoryGeneralDAL;
            }
            set
            {
                dataFactoryGeneralDAL = value;
            }
        }
        private string dataFactoryQueryDAL;
        public string DataFactoryQueryDAL
        {
            get
            {
                return dataFactoryQueryDAL;
            }
            set
            {
                dataFactoryQueryDAL = value;
            }
        }
    }
}
