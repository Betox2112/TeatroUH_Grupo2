using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace TeatroUH.Infrastructure.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.Set(key, Encoding.UTF8.GetBytes(json));
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            if (!session.TryGetValue(key, out var data) || data == null)
                return default;

            var json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
