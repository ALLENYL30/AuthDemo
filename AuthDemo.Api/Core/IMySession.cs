using Microsoft.Extensions.Caching.Memory;

namespace AuthDemo.Api.Core
{
    public interface IMySession
    {
        string GetString(string key);
        void InitSession(string sessionId, IMemoryCache cache);
        void SetString(string key, string value);
    }
}