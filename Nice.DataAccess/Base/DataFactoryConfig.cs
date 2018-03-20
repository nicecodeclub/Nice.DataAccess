namespace Nice.DataAccess
{
    public class DataFactoryConfig : DatabaseConfig
    {

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
