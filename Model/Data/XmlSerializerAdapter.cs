using System.IO;
using System.Xml.Serialization;

namespace Model.Data
{
    public class XmlSerializerAdapter<T> : AbstractSerializer<T>
    {
        public override void Serialize(string filePath, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, data);
        }

        public override T Deserialize(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StreamReader(filePath);
            return (T)serializer.Deserialize(reader)!;
        }
    }
}
