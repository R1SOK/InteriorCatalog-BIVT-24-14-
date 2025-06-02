namespace Model.Core
{
    public class Armchair : Chair
    {
        public bool IsReclining { get; set; }

        public override decimal Price => 2000 + (IsReclining ? 10000 : 0);

        public Armchair() { }
    }

}
