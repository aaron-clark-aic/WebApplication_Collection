using CSharpTest.Net.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_CQRestAPI.Models.Tool.Token
{
    /// <summary>
    /// 第二层结构 list中存放其实是一个容器用于存放同一个devicetype下的所有用户的第三层结构,第三层结构才是一个实体的结构,
    ///第二层的结构为一个非实体结构,限制list大小在配置文件中标明大小
    ///使用utoken作为dictionary的key
    /// </summary>
    public class UserDeviceMutability_mobile:UserDeviceMutability
    {
        //public DeviceType DeviceType { get; set; }

        private static UserDeviceMutability_mobile _instance;


        private UserDeviceMutability_mobile() {
            
        }


        static public UserDeviceMutability_mobile GetInstance() {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                _instance =  new UserDeviceMutability_mobile();
            }
            return _instance;
        }

        

    }

    public class UserDeviceMutability_pc : UserDeviceMutability
    {
        //public DeviceType DeviceType { get; set; }

        private static UserDeviceMutability_pc _instance;


        private UserDeviceMutability_pc() { }


        static public UserDeviceMutability_pc GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                _instance = new UserDeviceMutability_pc();
            }
            return _instance;
        }



    }


    /// <summary>
    /// 第三层结构,存储设备的基本信息包括设备类型和每个登录设备对应的token
    /// </summary>
    public class UserTokenMutability : TokenBase
    {
        public string Uid { get; set; }
        /// <summary>
        /// ECC算法的设备端公钥
        /// </summary>
        public string EccPublicKeyStr { get; set; }
        /// <summary>
        /// 用户token
        /// </summary>
        public string Utoken { get; set; }

        public UserTokenMutability(string eccPublicKey,string uid) {
           this.EccPublicKeyStr = eccPublicKey;
           this.Uid = uid;
           this.Utoken = SetToken();
        }
        /// <summary>
        /// 应该传入base的token
        /// </summary>
        /// <param name="token"></param>
        public override string SetToken()
        {
            return base.Token + "-" + GetUnixtimestamp();
        }

    }
}