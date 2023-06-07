using Newtonsoft.Json;

namespace FlowerClient
{
    public static class SessionExtensions
    {
        public static T GetData<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetData(this ISession session, string key, object value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            session.SetString(key, serializedValue);
        }
    }
}
