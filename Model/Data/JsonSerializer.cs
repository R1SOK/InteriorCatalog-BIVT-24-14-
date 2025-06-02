using System.IO;
using Newtonsoft.Json;

namespace Model.Data
{
    public class JsonSerializer<T> : AbstractSerializer<T>
    {
        public override void Serialize(string filePath, T data)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            File.WriteAllText(filePath, json);
        }

        public override T Deserialize(string filePath)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json, settings)!;
        }
    }
}
