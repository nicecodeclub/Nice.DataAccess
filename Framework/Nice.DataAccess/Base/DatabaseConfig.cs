namespace Nice.DataAccess
{
    public class DatabaseConfig
    {
        private string connStrKey;
        public string ConnStrKey
        {
            get { return connStrKey; }
            set { connStrKey = value; }
        }

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
    }
}
