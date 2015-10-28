using CSharpTest.Net.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication_CQRestAPI.Models.Tool.Token
{
    /// <summary>
    /// 第一层结构,list中存放UserDeviceMutability列表,使用deviceType作为key
    /// </summary>
    public class SingleUserTokenMutability
    {
        private static SingleUserTokenMutability _instance = null;

        /// <summary>
        /// UserDeviceMutability的键值对,使用devicetype作为key
        /// </summary>
        private LurchTable<string, UserDeviceMutability> _tokenDictionary_s;
        /// <summary>
        /// 构造函数中使用config文件初始化token缓存大小
        /// </summary>
        private  SingleUserTokenMutability() {
            
                //int _limit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LurchTableCache"]);
            //存放的数据应该是devicetype长度的
            int _limit = System.Enum.GetNames(typeof(DeviceType)).Length;
            
            //构造函数中处理缓存大小,从xml中配置
            _tokenDictionary_s = new LurchTable<string, UserDeviceMutability>(_limit,LurchTableOrder.Access);
        }

        public static SingleUserTokenMutability GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                _instance =  new SingleUserTokenMutability();
            }
            return _instance;
        }


        /// <summary>
        /// 更新lurchtable,维护最新的UserDeviceMutability的键值对,使用devicetype作为key
        /// </summary>
        /// <param name="tkey">devicetype</param>
        /// <param name="tbase">UserTokenMutability</param>
        /// <returns></returns>
        public bool UpdateTokenLurchtable(DeviceType tkey, UserDeviceMutability tbase) {
            //清理缓存
            //GCLurchtable();
            if (_tokenDictionary_s.ContainsKey(tkey.ToString()))
            {
                return _tokenDictionary_s.TryUpdate(tkey.ToString(), tbase);
            }
            else
            {
                return _tokenDictionary_s.TryAdd(tkey.ToString(), tbase);
            }
        }

        /// <summary>
        /// 移除不需要的UserDeviceMutability的键值对
        /// </summary>
        /// <param name="tkey">devicetype</param>
        /// <returns></returns>
        public bool DeleteTokenLurchtable(string tkey) {            
            if (_tokenDictionary_s.ContainsKey(tkey))
            {
                _tokenDictionary_s.Remove(tkey);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取UserDeviceMutability
        /// </summary>
        public UserDeviceMutability GetUserTokenMutabilityFromLurchtable(DeviceType dt)
        {
            UserDeviceMutability _ut = null;
            _tokenDictionary_s.TryGetValue(dt.ToString(),out _ut);
           

#if false
            //使用ling没有触发fetched事件
            return _tokenLurchtables.Where(a => a.Key == uidtokenKey).SingleOrDefault().Value.Uid;
#else
            return _ut;
#endif
        }
        

    }

    /// <summary>
    /// 登录的设备类型
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        ///手机
        /// </summary>
        Mobile = 0,
        /// <summary>
        /// 电脑
        /// </summary>
        PC = 1,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 2,
    }



}