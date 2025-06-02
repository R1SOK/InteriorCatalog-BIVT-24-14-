using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Model.Core
{
    [XmlRoot("FurnitureCatalog")]
    [XmlInclude(typeof(Chair))]
    [XmlInclude(typeof(Table))]
    [XmlInclude(typeof(Sofa))]
    [XmlInclude(typeof(Bed))]
    [XmlInclude(typeof(Stool))]
    [XmlInclude(typeof(Armchair))]
    public partial class FurnitureCatalog : IFurnitureCatalog, ISortable
    {
        public string Name { get; set; }
        public string Season { get; set; }

        [XmlArray("Items")]
        [XmlArrayItem(typeof(Chair))]
        [XmlArrayItem(typeof(Table))]
        [XmlArrayItem(typeof(Sofa))]
        [XmlArrayItem(typeof(Bed))]
        [XmlArrayItem(typeof(Stool))]
        [XmlArrayItem(typeof(Armchair))]
        public List<Furniture> Items { get; set; } = new();

        public FurnitureCatalog() { }

        // --- Методы IFurnitureCatalog ---
        public void Add(Furniture item) => Items.Add(item);

        public void Add(Furniture[] items) => Items.AddRange(items);

        public void Remove(string article) =>
            Items.RemoveAll(i => i.Article == article);

        public void Remove(Type type) =>
            Items.RemoveAll(i => i.GetType() == type);

        // --- Методы ISortable ---
        public void Sort(bool ascending = true) =>
            Items = ascending
                ? Items.OrderBy(i => i.Article).ToList()
                : Items.OrderByDescending(i => i.Article).ToList();

        public void SortByName(bool ascending = true) =>
            Items = ascending
                ? Items.OrderBy(i => i.ModelName).ToList()
                : Items.OrderByDescending(i => i.ModelName).ToList();

        public void SortByPrice(bool ascending = true) =>
            Items = ascending
                ? Items.OrderBy(i => i.Price).ToList()
                : Items.OrderByDescending(i => i.Price).ToList();

        public void PrioritySort() =>
            Items = Items
                .OrderBy(i => i.Brand)
                .ThenBy(i => i.ModelName)
                .ThenBy(i => i.Article)
                .ToList();
    }
}
