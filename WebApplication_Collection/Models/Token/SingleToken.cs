using CSharpTest.Net.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication_CQRestAPI.Models.Tool.Token
{
    public class SingleUserToken
    {
        private static SingleUserToken _instance = null;
        
        /// <summary>
        /// token的键值对
        /// </summary>
        private LurchTable<string, UserToken> _tokenLurchtables;
        /// <summary>
        /// 构造函数中使用config文件初始化token缓存大小
        /// </summary>
        public SingleUserToken() {
                int _limit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LurchTableCache"]);
            //防止出现意外没有找到配置的问题
            if (_limit<1) {
                _limit = 5;
            }
            //构造函数中处理缓存大小,从xml中配置
            _tokenLurchtables = new LurchTable<string, UserToken>(LurchTableOrder.Access,_limit);
        }

        public static SingleUserToken GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                return new SingleUserToken();
            }
        }


        /// <summary>
        /// 更新lurchtable,维护最新的token
        /// </summary>
        /// <param name="tkey"></param>
        /// <param name="tbase"></param>
        /// <returns></returns>
        public bool UpdateTokenLurchtable(string tkey, UserToken tbase) {
            //清理缓存
            //GCLurchtable();
            if (_tokenLurchtables.ContainsKey(tkey))
            {
                return _tokenLurchtables.TryUpdate(tkey, tbase);
            }
            else
            {
                return _tokenLurchtables.TryAdd(tkey, tbase);
            }
        }
       
        /// <summary>
        /// 移除不需要的token
        /// </summary>
        /// <param name="tkey"></param>
        /// <returns></returns>
        public bool DeleteTokenLurchtable(string tkey) {            
            if (_tokenLurchtables.ContainsKey(tkey))
            {
                _tokenLurchtables.Remove(tkey);
                return true;
            }
            else
            {
                return false;
            }
        }
       
        /// <summary>
        /// 获取用户id
        /// </summary>
        public string GetUidFromLurchtable(string uidtokenKey)
        {
            UserToken _ut = null;
            _tokenLurchtables.TryGetValue(uidtokenKey,out _ut);

#if false
            //使用ling没有触发fetched事件
            return _tokenLurchtables.Where(a => a.Key == uidtokenKey).SingleOrDefault().Value.Uid;
#else
            return _ut.Uid;
#endif
        }
       

    }
}