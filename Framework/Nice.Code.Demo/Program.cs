using Nice.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.Code.Demo
{
    class Program
    {
        private static UserInfoRepository rp = null;
        static void Main(string[] args)
        {
            DatabaseConfig config = new DatabaseConfig();
            config.ConnStrKey = "NiceConnString";
            config.ConnString = ConfigurationManager.ConnectionStrings["NiceConnString"].ConnectionString;
            config.ProviderName = ConfigurationManager.ConnectionStrings["NiceConnString"].ProviderName;

            DataUtil.Create(config);

            rp = new UserInfoRepository();
            IList<UserInfo> userList = rp.GetList();

            Console.ReadLine();
        }


    }
}
