namespace Model.Core
{
    public class Stool : Chair
    {
        public bool IsSoft { get; set; }
        public Stool() { }
        public override decimal Price => 2500 + (IsSoft ? 3000 : 0);
    }
}
