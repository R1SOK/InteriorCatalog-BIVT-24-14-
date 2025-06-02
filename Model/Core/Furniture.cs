using System.Xml.Serialization;

namespace Model.Core
{

    public abstract class Furniture
    {
        public string Id { get; set; }
        public string Article { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public string ImagePath { get; set; }
        
        [XmlIgnore]
        public abstract decimal Price { get; }
        public static decimal operator +(Furniture a, Furniture b)
        {
            if (a == null || b == null) return 0;
            return a.Price + b.Price;
        }
        

    }
}
