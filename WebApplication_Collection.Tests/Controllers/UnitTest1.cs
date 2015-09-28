using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication_CQRestAPI.Models.Tool.Token;
using WebApplication_Collection.Models.RSA;
using System.Security.Cryptography;
using System.IO;

namespace WebApplication_Collection.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestUserTokenMutability()
        {
            //初始化第一层结构
            SingleUserTokenMutability stm = SingleUserTokenMutability.GetInstance();
#if true
            //>1
            string utoken_1_mobile = "";
            string utoken_1_pc = "";
#endif


            int i = 0;
            do
            {
                //建立一个用户token送入一个device类型的第二级结构内存
                UserTokenMutability ut_mobile = new UserTokenMutability("eccPK_mobile" + i, "userId" + i);
                UserDeviceMutability ud_mobile = DeivceSelected.GetDeivceInstance(DeviceType.Mobile);
                ud_mobile.UpdateTokenLurchtable(ut_mobile.Utoken, ut_mobile);
                stm.UpdateTokenLurchtable(DeviceType.Mobile, ud_mobile);

                //pc
                UserTokenMutability ut_pc = new UserTokenMutability("eccPK_pc" + i, "userId" + i);
                UserDeviceMutability ud_pc = DeivceSelected.GetDeivceInstance(DeviceType.PC);
                ud_pc.UpdateTokenLurchtable(ut_pc.Utoken, ut_pc);
                stm.UpdateTokenLurchtable(DeviceType.PC, ud_pc);

#if true
                //>1 测试调用token时是否刷新了调用顺序 测试lru

                if (i == 1)
                {
                    utoken_1_mobile = ut_mobile.Utoken;
                }
                if (i == 3)
                {
                    utoken_1_pc = ut_pc.Utoken;
                }

                if (i > 3)
                {
                    Console.Write("get mobile uid = " + stm.GetUserTokenMutabilityFromLurchtable(DeviceType.Mobile).GetUidFromLurchtable(utoken_1_mobile));
                    Console.Write("get pc uid = " + stm.GetUserTokenMutabilityFromLurchtable(DeviceType.PC).GetUidFromLurchtable(utoken_1_pc));

                }
#endif


                //循环变量
                i++;

            } while (i < 10);

            SingleUserTokenMutability _stm = stm;
        }


        /// <summary>
        ///  测试写入rsa的xml文件
        /// </summary>
        [TestMethod]
        public void TestRsaWriteXmlFile() {

            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //string pu_str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + System.Configuration.ConfigurationManager.AppSettings["RSA_xmlFile_address_private"];
            //string pr_str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + System.Configuration.ConfigurationManager.AppSettings["RSA_xmlFile_address_public"];
            //using (StreamWriter writer = new StreamWriter(pr_str))  //这个文件要保密...
            //{

            //    writer.WriteLine(rsa.ToXmlString(true));

            //}
            //using (StreamWriter writer = new StreamWriter(pu_str))
            //{

            //    writer.WriteLine(rsa.ToXmlString(false));

            //}

            string _keyStr = Convert.ToString( RSACache.DEF_PK);
        }

    }
}
