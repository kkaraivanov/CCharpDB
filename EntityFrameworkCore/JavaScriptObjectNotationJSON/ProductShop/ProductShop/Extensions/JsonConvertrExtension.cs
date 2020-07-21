namespace ProductShop.Extensions
{
    using Newtonsoft.Json;

    public static class JsonConvertrExtension
    {
        public static T[] DeserializeObject<T>(this string inputJson, JsonSerializerSettings settings = null) where T : class
        {
            return settings != null ?
                JsonConvert.DeserializeObject<T[]>(inputJson, settings) :
                JsonConvert.DeserializeObject<T[]>(inputJson);
        }

        public static string SerializeObject<T>(this T[] inputJson, JsonSerializerSettings settings) where T : class
        {
            return JsonConvert.SerializeObject(inputJson, settings);
        }

        public static string SerializeObject<T>(this T inputJson, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(inputJson, settings);
        }

        public static string SerializeObject<T>(this T[] inputJson, Formatting format) where T : class
        {
            return JsonConvert.SerializeObject(inputJson, format);
        }

        public static string SerializeObject<T>(this T inputJson, Formatting format)
        {
            return JsonConvert.SerializeObject(inputJson, format);
        }
    }
}