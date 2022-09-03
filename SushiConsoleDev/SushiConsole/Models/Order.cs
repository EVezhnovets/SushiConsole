using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Models
{
    public class Order
    {
        public event Action<object, EventArgs> OrderIsPacked;
        public event Action<object, EventArgs> OrderIsDelivered;
        public event Action<object, EventArgs> OrderIsPaid;

        public int Id { get; private set; }
        public Client Client { get; set; }
        public Dictionary<int, Item> OrderItems { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsPaid { get; set; }
        public static int CounterOrder { get; set; } = 1;

        public Order(Client client)
        {
            Id = CounterOrder;
            OrderItems = new Dictionary<int, Item>();
            Client = client;
            CounterOrder++;
        }

        public void ShowOrderItems()
        {
            foreach (var item in OrderItems)
            {
                Console.WriteLine($"{item.Value.Name}");
            }
        }

        public bool ToPackOrder(bool prop, TimeSpan time)
        {
            if (prop)
            {
                IsCompleted = prop;
            }
            Console.WriteLine($"Ваш заказ будет скомплектован через {time} секунд");
            Thread.Sleep(time);
            OrderIsPacked?.Invoke(this, new OrderEventsArgs("Заказ укомплектован", IsCompleted));
            return IsCompleted;
        }
        
        public bool ToDeliver(bool prop, TimeSpan time)
        {
            
            if (prop)
            {
                IsDelivered = prop;
            }
            Console.WriteLine($"Ваш заказ будет доставлен в течении{time}");
            Thread.Sleep(time);
            OrderIsDelivered?.Invoke(this, new OrderEventsArgs("Заказ доставлен", IsDelivered));
            return IsDelivered; 
        }

        public bool ToPay(bool prop, TimeSpan time)
        {

            if (prop)
            {
                IsPaid = prop;
            }
            Console.WriteLine($"Ваш заказ оплачен");
            OrderIsPaid?.Invoke(this, new OrderEventsArgs("Заказ оплачен", IsPaid));
            return IsPaid;
        }
    }
}