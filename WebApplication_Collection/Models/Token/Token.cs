using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_CQRestAPI.Models.Tool.Token
{
    public class TokenBase
    {
        public string Token { get; set; }

        /// <summary>
        /// 生成GUID,由于是base方法统一使用最原生的生成方式,在上一层上面做特性化修改,使用构造函数的调用顺序来执行生成[aaron][20150911]
        /// </summary>
        /// <returns></returns>
        private string GenerateGUID() {
            return System.Guid.NewGuid().ToString();
        }

        public TokenBase() {
            this.Token = GenerateGUID();
        }

        /// <summary>
        /// 用于在子类中替换掉base中的标准token
        /// </summary>
        /// <param name="token"></param>
        public virtual string SetToken() {
            return Token;
        }

        /// <summary>
        /// 获取unixtimestamp
        ///  当此方法被overwrite时可以延伸出各种加盐的token,所以不需要在意方法名,该方法名只是为了默认的是实现
        /// </summary>
        /// <returns></returns>
        public long GetUnixtimestamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));

            DateTime nowTime = DateTime.Now;

            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }
    }

}