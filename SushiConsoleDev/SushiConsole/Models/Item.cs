using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Models
{
    public class Item
    {
        public int Id{ get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double Calories { get; set; }
        public decimal Price { get; set; }  

        //to delete
        public static int CountItem { get; set; } = 1;

        public Item(string name, string description, decimal price)
        {
            Id = CountItem;
            Name = name;
            Description = description;
            Price = price;
            CountItem++;
        }
    }
}