using Microsoft.Extensions.Configuration;
using Nice.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Nice.Code.Demo
{
    class Program
    {
        private static UserInfoRepository rp = null;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json", true, true);
            IConfigurationRoot Configuration = builder.Build();


            DataUtil.Create(new DatabaseConfig()
            {
                ConnString = Configuration["ConnString"],
                ProviderName = Configuration["ProviderName"]
            });

            rp = new UserInfoRepository();
            TestInsert();
            TestUpdate();
            TestGetList();
            TestDelete();
            Console.ReadLine();
        }

        static void TestInsert()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.UserId = Guid.NewGuid().ToString("N");
            userInfo.UserName = "test";
            userInfo.RealName = "小明";
            userInfo.SPassword = "123456";
            userInfo.SEmail = "xiaoming@qq.com";
            userInfo.STelephone = "18234309987";
            userInfo.SRemark = "测试";
            userInfo.CreateTime = DateTime.Now;
            userInfo.ModifyTime = userInfo.CreateTime;
            userInfo.CreateUserId = userInfo.UserId;
            userInfo.ModifyUserId = userInfo.UserId;
            userInfo.NState = 1;
            bool result = rp.Insert(userInfo);
            Console.WriteLine("新增用户{0}", result ? "成功" : "失败");

            userInfo.SRemark = "新增或更新用户";
            userInfo.CreateTime = DateTime.Now;
            result = rp.InsertOrUpdate(userInfo);
            Console.WriteLine("新增或更新用户{0}", result ? "成功" : "失败");
        }

        static void TestUpdate()
        {
            //UserInfo userInfo = rp.Get("56bf9714c0f743c9acced48b4352fd34");
            IList<UserInfo> userList = rp.GetList();
            UserInfo userInfo = userList.FirstOrDefault();
            if (userInfo != null)
            {
                userInfo.SPassword = "897099";
                userInfo.ModifyTime = DateTime.Now;
                bool result = rp.Update(userInfo);
                Console.WriteLine("修改用户{0}", result ? "成功" : "失败");
                userInfo = rp.GetByName(userInfo.UserName);
                if (userInfo != null)
                {
                    userInfo.UserName = "sssss";
                    userInfo.ModifyTime = DateTime.Now;
                    result = rp.UpdateState(userInfo);
                    Console.WriteLine("修改用户{0}", result ? "成功" : "失败");

                    userInfo.UserName = "456789";
                    userInfo.ModifyTime = DateTime.Now;
                    result = rp.Update(userInfo, o => o.UserName, o => o.ModifyTime);
                    Console.WriteLine("修改用户{0}", result ? "成功" : "失败");
                }
            }
        }

        static void TestGetList()
        {
            IList<UserInfo> userList = rp.GetList();
            userList = rp.GetList(new DataAccess.Model.Page.PageInfo(0, 10));
            userList = rp.GetList(o => o.UserName == "test");

            userList = rp.GetList(o => o.UserName == "test", new DataAccess.Model.Page.PageInfo(0, 10));
        }

        static void TestDelete()
        {
            IList<UserInfo> userList = rp.GetList(new DataAccess.Model.Page.PageInfo(0, 10));

            UserInfo userInfo = userList.Last();
            UserInfo firstuserInfo = userList.First();
            bool result = rp.VirtualDelete(userInfo);
            result = rp.VirtualDelete(firstuserInfo.UserId);

            result = rp.Delete(userInfo);
            result = rp.Delete(firstuserInfo.UserId);
        }
        static void TestTrans()
        {
            UserInfo userInfo = rp.Get("27428467b52e4a3bac792c42e3d65891");
            using (TransactionScope transactionScope1 = new TransactionScope())
            {
                try
                {
                    TestInsert();
                    using (TransactionScope transactionScope2 = new TransactionScope())
                    {
                        rp.UpdateErrorTest(userInfo);
                        transactionScope2.Complete();
                    }
                    transactionScope1.Complete();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
