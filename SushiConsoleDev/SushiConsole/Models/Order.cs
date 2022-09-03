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
        public List<Tuple<int, Item>> OrderItems { get; set; }
        public string CardNumber { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsPaid { get; set; }
        public static int CounterOrder { get; set; } = 1;

        public Order(Client client)
        {
            Id = CounterOrder;
            OrderItems = new List<Tuple<int, Item>>();
            Client = client;
            CounterOrder++;
        }

        public decimal ShowOrderItems()
        {
            List<decimal> sumOfPositionList = new List<decimal>();
            decimal sumOfPosition = default;
            decimal sumOfAllPositions = default;
            foreach (var item in OrderItems)
            {
                Console.WriteLine($"{item.Item2.Name} - {item.Item1} штук, {sumOfPosition = item.Item2.Price * item.Item1} byn");
                sumOfPositionList.Add(sumOfPosition);
            }
            foreach (var item in sumOfPositionList)
            {
                sumOfAllPositions += item;
            }
            return sumOfAllPositions;
        }

        public bool ToPackOrder(bool prop/*, TimeSpan time*/)
        {
            if (prop)
            {
                IsCompleted = prop;
            }
            Console.WriteLine($"Ваш заказ комплектуется");
            OrderIsPacked?.Invoke(this, new OrderEventsArgs("Заказ укомплектован", IsCompleted));
            return IsCompleted;
        }
        
        public bool ToDeliver(bool prop/*, TimeSpan time*/)
        {
            
            if (prop)
            {
                IsDelivered = prop;
            }
            Console.WriteLine($"Ваш заказ будет доставлен в течении  секунд");
            OrderIsDelivered?.Invoke(this, new OrderEventsArgs("Заказ доставлен", IsDelivered));
            return IsDelivered; 
        }

        public bool ToPay(bool prop/*, TimeSpan time*/)
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