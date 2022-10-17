namespace SushiConsoleDev.Models
{
    public class Item
    {
        //private int _id;
        //private string _name;
        //private string _compound;
        //private double _weight;
        //private double _calories;
        //private decimal _price;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Compound { get; set; }
        public decimal Price { get; set; }

        public static int CountItem { get; set; } = 1;

        public Item(string name, string compound, decimal price)
        {
            Id = CountItem;
            Name = name;
            Compound = compound;
            Price = price;
            CountItem++;
        }
    }
}