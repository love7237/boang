using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace T.Utility.Serialization
{
    /// <summary>
    /// JSON序列化封装类
    /// </summary>
    public static class JsonHelper
    {
        static JsonHelper()
        {
            //文档地址：https://www.newtonsoft.com/json/help/html/SerializationSettings.htm
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                JsonSerializerSettings setting = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss",
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
                return setting;
            });
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="indented">True to be indented, the default value is false.</param>
        /// <returns></returns>
        public static string Serialize(object value, bool indented = false)
        {
            return JsonConvert.SerializeObject(value, indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting and customized <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">customized settings</param>
        /// <param name="indented">true to be indented, the default value is false</param>
        /// <returns></returns>
        public static string Serialize(object value, JsonSerializerSettings settings, bool indented = false)
        {
            return JsonConvert.SerializeObject(value, indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None, settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using customized <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The object to deserialize.</param>
        /// <param name="settings">The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object. If this is null, default serialization settings will be used.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string value, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="node">节点名称</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json, string node)
        {
            return Deserialize<T>(GetPropertyString(json, node));
        }

        /// <summary>
        /// Deserializes the JSON to the given anonymous type.
        /// </summary>
        /// <typeparam name="T">The anonymous type to deserialize to. This can't be specified traditionally and must be inferred from the anonymous type passed as a parameter.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <returns></returns>
        public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
        {
            return JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject);
        }

        /// <summary>
        /// Deserializes the JSON to the given anonymous type using customized <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <param name="settings">The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object. If this is null, default serialization settings will be used.</param>
        /// <returns></returns>
        public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject, settings);
        }

        /// <summary>
        /// Get the specified JSON object property string with formatting.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="indented">Causes child objects to be indented.</param>
        /// <returns></returns>
        public static string GetPropertyString(string json, string propertyName, bool indented = false)
        {
            return JObject.Parse(json).GetValue(propertyName).ToString(indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);
        }

        /// <summary>
        /// Formatting the JSON string with indented.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="indented">Causes child objects to be indented.</param>
        /// <returns></returns>
        public static string Formatting(string json, bool indented = true)
        {
            object obj = JsonConvert.DeserializeObject(json);
            string formatString = JsonConvert.SerializeObject(obj, indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);
            return formatString;
        }

    }
}
