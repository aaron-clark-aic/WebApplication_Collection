using CSharpTest.Net.Collections;
using System;
using System.Collections.Generic;

namespace WebApplication_CQRestAPI.Models.Tool.Token
{
    /// <summary>
    /// 设备的基类
    /// </summary>
    public class UserDeviceMutability
    {
        /// <summary>
        /// 存放第三级结构
        /// </summary>
        protected LurchTable<string, UserTokenMutability> _userTokenMutability_s;

        protected UserDeviceMutability()
        {
            //配置文件中获取内存大小,用于存放的记录数量上限
            int _limit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LurchTableCache"]);
            //防止出现意外没有找到配置的问题
            if (_limit < 1)
            {
                _limit = 5;
            }
            _userTokenMutability_s = new LurchTable<string, UserTokenMutability>(LurchTableOrder.Access, _limit);
        }

        /// <summary>
        /// 更新lurchtable,维护最新的deivcelist
        /// </summary>
        /// <param name="utoken"></param>
        /// <param name="tbase"></param>
        /// <returns></returns>
        public bool UpdateTokenLurchtable(string utoken, UserTokenMutability tbase)
        {

            if (_userTokenMutability_s.ContainsKey(utoken.ToString()))
            {
                return _userTokenMutability_s.TryUpdate(utoken.ToString(), tbase);
            }
            else
            {
                return _userTokenMutability_s.TryAdd(utoken.ToString(), tbase);
            }
        }

        // <summary>
        // 移除不需要的token
        // </summary>
        // <param name = "tkey" ></ param >
        // < returns ></ returns >
        public bool DeleteTokenLurchtable(string utoken)
        {
            if (_userTokenMutability_s.ContainsKey(utoken))
            {
                _userTokenMutability_s.Remove(utoken);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 获取指定utoken对应的用户uid
        /// </summary>
        /// <param name="utoken"></param>
        /// <returns></returns>
        public string GetUidFromLurchtable(string utoken)
        {
            UserTokenMutability _md = null;
            _userTokenMutability_s.TryGetValue(utoken, out _md);
            return _md.Uid;
        }
    }
}