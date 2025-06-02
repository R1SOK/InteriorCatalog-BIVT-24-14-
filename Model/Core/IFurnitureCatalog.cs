using System;

namespace Model.Core
{
    public interface IFurnitureCatalog
    {
        void Add(Furniture item);
        void Add(Furniture[] items);
        void Remove(string article);
        void Remove(Type type);
    }
}
