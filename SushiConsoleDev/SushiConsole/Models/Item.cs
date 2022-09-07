using SushiConsoleDev.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SushiConsole.Models
{
    public class Item
    {
        Type type = typeof(Item);
        private int _id;
        private string _name;
        private string _description;
        private double _weight;
        private double _calories;
        private decimal _price;

        public int Id
        {
            get
            {
                Logger.Info(Logger.info, type, "Id", "Call get method", $"{Logger.ThreadInfo}");

                return _id;
            } 
            private set
            {
                _id = value;

                Logger.Info(Logger.info, type, "Id", "Call set method", $"{Logger.ThreadInfo}");

            }
        }
        public string Name
        {
            get
            {
                return _name;

                Logger.Info(Logger.info, type, "Name", "Call get method", $"{Logger.ThreadInfo}");

            }
            set
            {
                Logger.Info(Logger.info, type, "Name", "Call set method", $"{Logger.ThreadInfo}");

                _name = value;
            }
        }
        public string Description
        {
            get
            {
                Logger.Info(Logger.info, type, "Description", "Call get method", $"{Logger.ThreadInfo}");

                return _description;
            }
            set
            {
                _description = value;

                Logger.Info(Logger.info, type, "Description", "Call set method", $"{Logger.ThreadInfo}");
            }
        }
        public double Weight
        {
            get
            {
                Logger.Info(Logger.info, type, "Weight", "Call get method", $"{Logger.ThreadInfo}");

                return _weight;
            }
            set
            {
                _weight = value;

                Logger.Info(Logger.info, type, "Weight", "Call set method", $"{Logger.ThreadInfo}");
            }
        }
        public double Calories
        {
            get
            {
                Logger.Info(Logger.info, type,"Calories", "Call get method", $"{Logger.ThreadInfo}");

                return _calories;
            }
            set
            {
                _calories = value;

                Logger.Info(Logger.info, type, "Calories", "Call set method", $"{Logger.ThreadInfo}");
            }
        }
        public decimal Price
        {
            get
            {
                Logger.Info(Logger.info, type, "Price", "Call get method", $"{Logger.ThreadInfo}");

                return _price;
            }
            set
            {
                _price = value;

                Logger.Info(Logger.info, type, "Price", "Call set method", $"{Logger.ThreadInfo}");
            }
        }

        //to delete
        public static int CountItem { get; set; } = 1;

        public Item(string name, string description, decimal price)
        {
            Id = CountItem;
            Name = name;
            Description = description;
            Price = price;
            CountItem++;

            Logger.Info(Logger.info, type, "Price", "Call constructor", $"{Logger.ThreadInfo}");
        }

        
    }
}