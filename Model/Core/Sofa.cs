namespace Model.Core
{
    public class Sofa : Furniture
    {
        public int Seats { get; set; }
        public bool HasStorage { get; set; }
        public Sofa() { }
        public override decimal Price => 10000 + Seats * 2000 + (HasStorage ? 4000 : 0);
    }
}
