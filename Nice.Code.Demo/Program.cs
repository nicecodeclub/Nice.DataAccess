using Nice.DataAccess;
using Nice.DataAccess.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nice.Code.Demo
{
    class Program
    {
        private static UserInfoRepository rp = null;
        static void Main(string[] args)
        {
            DataUtil.Create();
            rp = new UserInfoRepository();
            //TestInsert();
            //TestUpdate();
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
            }
        }
    }
}
