using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace T.Utility.Serialization
{
    /// <summary>
    /// XML序列化封装类
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xml">字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="stream">流</param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            return (T)xmldes.Deserialize(stream);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="indent">true 换行和缩进;false 不换行缩进</param>
        /// <returns></returns>
        public static string Serialize(object obj, bool indent)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = indent,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(writer, obj, namespaces);
                    writer.Flush();

                    //注意：以上过程生成的xml头部会多一个空白字符，需要去除
                    //      https://blog.csdn.net/hawksoft/article/details/54139876
                    string xml = Encoding.UTF8.GetString(ms.ToArray());
                    int index = xml.IndexOf('<');
                    xml = xml.Substring(index);
                    return xml;
                }
            }
        }

    }
}
