using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class Cache
    {
        private static CacheItemPolicy Policy = new CacheItemPolicy()
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(10)
        };

        private static MemoryCache Instance => MemoryCache.Default;

        public static T TryGet<T>(Func<T> getData, string key) where T : class
        {
            var data = (T) Instance[key];

            if (data == null)
            {
                data = getData.Invoke();

                Instance.Set(key, data, Policy);
            }

            return data;
        }
    }
}
