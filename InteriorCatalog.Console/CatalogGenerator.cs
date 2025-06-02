using System;
using System.Collections.Generic;
using Model.Core;
using Model.Data;

namespace InteriorCatalog.ConsoleApp
{
    // 1. Single Responsibility Principle - каждый класс имеет одну ответственность
    
    public interface ICatalogGenerator
    {
        void GenerateAll(string format);
    }

    public interface IFurnitureCatalogFactory
    {
        FurnitureCatalog Create(string name, List<Furniture> items);
    }

    public interface ICatalogSerializer
    {
        void Serialize(string filename, FurnitureCatalog catalog);
    }
    public class FurnitureCatalogFactory : IFurnitureCatalogFactory
    {
        public FurnitureCatalog Create(string name, List<Furniture> items)
        {
            return new FurnitureCatalog
            {
                Name = name.Replace("catalog-", "") + " Catalog",
                Season = name.Contains("summer") ? "Summer" : "Other",
                Items = items
            };
        }
    }

    public class CatalogSerializer : ICatalogSerializer
    {
        private readonly AbstractSerializer<FurnitureCatalog> _serializer;

        public CatalogSerializer(string format)
        {
            _serializer = format.ToLower() switch
            {
                "json" => new JsonSerializer<FurnitureCatalog>(),
                "xml" => new XmlSerializerAdapter<FurnitureCatalog>(),
                _ => throw new ArgumentException("Unsupported format")
            };
        }

        public void Serialize(string filename, FurnitureCatalog catalog)
        {
            _serializer.Serialize(filename, catalog);
        }
    }

    public class CatalogGenerator : ICatalogGenerator
    {
        private readonly IFurnitureCatalogFactory _catalogFactory;
        private readonly Func<string, ICatalogSerializer> _serializerFactory;

        public CatalogGenerator(
            IFurnitureCatalogFactory catalogFactory,
            Func<string, ICatalogSerializer> serializerFactory)
        {
            _catalogFactory = catalogFactory;
            _serializerFactory = serializerFactory;
        }

        public void GenerateAll(string format)
        {
            var catalogs = GetCatalogDefinitions();
            var serializer = _serializerFactory(format);

            foreach (var (name, items) in catalogs)
            {
                var catalog = _catalogFactory.Create(name, items);
                string filename = $"{name}.{format.ToLower()}";
                serializer.Serialize(filename, catalog);
            }
        }

        // приватный метод получения определений каталога
        private List<(string Name, List<Furniture> Items)> GetCatalogDefinitions()
        {
            return new List<(string Name, List<Furniture> Items)>
            {
                ("catalog-summer", new List<Furniture>
                {
                    
                }),
            };
        }
    }
}