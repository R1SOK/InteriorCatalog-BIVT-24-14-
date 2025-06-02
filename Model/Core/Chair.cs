namespace Model.Core
{
    public class Chair : Furniture
    {
        public bool HasBackSupport { get; set; }
        public int Legs { get; set; }

        public override decimal Price => 1000 + (HasBackSupport ? 5000 : 0) + Legs * 1000;

        public Chair() { }
    }

}
