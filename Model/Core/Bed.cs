namespace Model.Core
{
    public class Bed : Furniture
    {
        public string Size { get; set; } = "Single";
        public bool HasHeadboard { get; set; }
        public bool IsBunk { get; set; } 
        public Bed() { }

        public override decimal Price
        {
            get
            {
                decimal sizePrice = Size switch
                {
                    "Single" => 0,
                    "Double" => 5000,
                    "Queen" => 10000,
                    "King" => 30000,
                    _ => 0
                };
                return 5000 + sizePrice + (HasHeadboard ? 10000 : 0);
            }
        }
    }
}