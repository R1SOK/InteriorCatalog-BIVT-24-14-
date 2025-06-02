using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Model.Core;

namespace InteriorCatalog.Console
{
    public static class CatalogLoader
    {
        // Загрузка из JSON
        public static FurnitureCatalog LoadFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<FurnitureCatalog>(json,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        // Сохранение в JSON
        public static void SaveToJson(FurnitureCatalog catalog, string filePath)
        {
            string json = JsonConvert.SerializeObject(catalog, Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            File.WriteAllText(filePath, json);
        }

        // Загрузка из XML
        public static FurnitureCatalog LoadFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(FurnitureCatalog), new[]
{
    typeof(Chair),
    typeof(Table),
    typeof(Sofa),
    typeof(Bed),
    typeof(Stool),
    typeof(Armchair)
});
            using var stream = new FileStream(filePath, FileMode.Open);
            return (FurnitureCatalog)serializer.Deserialize(stream);
        }

        // Сохранение в XML
        public static void SaveToXml(FurnitureCatalog catalog, string filePath)
        {
            var serializer = new XmlSerializer(typeof(FurnitureCatalog), new[]
            {
                typeof(Chair),
                typeof(Table),
                typeof(Sofa),
                typeof(Bed),
                typeof(Stool),
                typeof(Armchair)
            });
            using var stream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(stream, catalog);
        }
    }
}
