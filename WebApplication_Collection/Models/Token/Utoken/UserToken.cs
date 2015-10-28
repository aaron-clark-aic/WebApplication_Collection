using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_CQRestAPI.Models.Tool.Token
{
    public class UserToken: TokenBase
    {
        public string Uid { get; set; }
        public string Utoken { get; set; }
        /// <summary>
        /// ECC算法的设备端公钥
        /// </summary>
        public string EccPublicKeyStr { get; set; }

        public UserToken(string uid) {
            this.Uid = uid;
            this.Utoken = SetToken();
        }
       
        /// <summary>
        /// 应该传入base的token
        /// </summary>
        /// <param name="token"></param>
        public override string SetToken()
        {
            
            return base.Token+"-"+ GetUnixtimestamp();
        }

        /// <summary>
        /// 获取unixtimestamp
        /// </summary>
        /// <returns></returns>
        public long GetUnixtimestamp() {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));

            DateTime nowTime = DateTime.Now;

            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }
    }

   
}