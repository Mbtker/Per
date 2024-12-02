using System;
using System.Runtime.Caching;

namespace Performance.Management.BLL.Helper
{
    public class InMemoryCache : ICacheService
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, int minutes = 30) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();

                if (item != null)
                {
                    _ = MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(minutes));
                }
            }
            return item;
        }
    }

    interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, int minutes = 30) where T : class;
    }

}
