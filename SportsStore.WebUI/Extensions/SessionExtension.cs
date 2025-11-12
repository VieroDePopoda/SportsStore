using System.Text.Json;

namespace SportsStore.WebUI.Extensions
{
	public static class SessionExtensions
	{
		public static void SetObject<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		public static T? GetObject<T>(this ISession session, string key) where T : class
		{
			var value = session.GetString(key);
			return value == null ? null : JsonSerializer.Deserialize<T>(value);
		}
	}
}
