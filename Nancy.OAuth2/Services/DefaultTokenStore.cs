using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Nancy.OAuth2.Services
{   
    public interface IAccessTokenStore
    {
        void Store(string key, string value);

        void Remove(string key);

        string Retrieve(string key);
    }

    public class DefaultAccessTokenStore : IAccessTokenStore
    {
        private readonly MemoryCache cache;

        public DefaultAccessTokenStore()
        {
            this.cache = new MemoryCache("Nancy-AuthorizationTokenCache");
        }

        public void Store(string key, string value)
        {
            this.cache.Add(key, value, DateTimeOffset.MaxValue);
        }

        public void Remove(string key)
        {
            if (this.cache.Contains(key))
            {
                this.cache.Remove(key);
            }
        }

        public string Retrieve(string key)
        {
            return this.cache.Contains(key) ? (string)this.cache[key] : null;
        }
    }
}
