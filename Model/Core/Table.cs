namespace Model.Core
{
    public class Table : Furniture
    {
        public int Area { get; set; }
        public bool IsFolding { get; set; }
        public Table() { }

        public override decimal Price => 2000 + Area * 200 + (IsFolding ? 1000 : 0);
    }
}
