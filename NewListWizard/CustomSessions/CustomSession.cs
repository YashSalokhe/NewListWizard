﻿

namespace NewListWizard.CustomSessions
{
    public static class customSession
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            // Serialze the 'value' into JSON form and save it
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            // Get the Object with deserialization

            string value = session.GetString(key);
            if (value == null)
                // the default is an operator to provide default instance of th type
                return default(T); // return an empty instance

            // get the object from the session
            return JsonSerializer.Deserialize<T>(value);

        }
    }
}
