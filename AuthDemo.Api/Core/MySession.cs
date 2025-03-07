using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace AuthDemo.Api.Core
{


    public class MySession : IMySession
    {
        private string sessionId;
        private IMemoryCache cache;
        /// <summary>
        /// 初始化Session，在鉴权验证方法中被调用
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="cache"></param>
        public void InitSession(string sessionId, IMemoryCache cache)
        {
            this.cache = cache;
            this.sessionId = sessionId;
        }

        /// <summary>
        /// 通过key获取存储的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            // 1. 获取SessionId 并更具SessionId 拿到当前用户存储的键值对
            if (cache.TryGetValue(sessionId, out Dictionary<string, string> dic))
            {
                return dic[key];
            }
            return default;
        }


        public void SetString(string key, string value)
        {
            if (cache.TryGetValue(sessionId, out Dictionary<string, string> dic))
            {
                dic[key] = value;
            }
        }
    }
}