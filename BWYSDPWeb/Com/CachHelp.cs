using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

namespace BWYSDPWeb.Com
{
    public class CachHelp
    {
        ObjectCache cache = MemoryCache.Default;
        public CachHelp()
        {

        }

        public void AddCachItem(string key,object val)
        {
            var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(60) };
            cache.Set(key, val, policy);
        }

        public object GetCach(string key)
        {
            if (cache.Contains(key))
            {
                return cache[key];
            }
            return null;
        }
    }
}