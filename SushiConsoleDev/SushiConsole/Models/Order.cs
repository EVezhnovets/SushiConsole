using SushiConsoleDev.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Models
{
    public class Order
    {
        Type type = typeof(Order);

        private int _id;
        private Client _client;
        private List<Tuple<int, Item>> _orderItems;
        private string _cardNumber;
        private bool _isCompleted;
        private bool _isDelivered;
        private bool _isPaid;
        public static int maxNumberOfRollsPerOrder = 50;

        public event Action<object, EventArgs>? OrderIsPacked;
        public event Action<object, EventArgs>? OrderIsDelivered;
        public event Action<object, EventArgs>? OrderIsPaid;

        public int Id
        {
            get
            {
                if (_id != null)
                {
                    Logger.Info(Logger.info, type, "Id", "Call get method", $"{Logger.ThreadInfo}");
                    return (int)_id;
                }
                Logger.Error(Logger.error, type, "Id", "Call get method", $"{Logger.ThreadInfo}");
                throw new ArgumentNullException("Id is null");
            }
            private set
            {
                _id = value;
                Logger.Info(Logger.info, type, "Id", "Call set method", $"{Logger.ThreadInfo}");
            }
        }
        public Client Client 
        { 
            get
            {
                Logger.Info(Logger.info, type, "Client", "Call get method", $"{Logger.ThreadInfo}");

                return _client ;
            }
            set
            {
                Logger.Info(Logger.info, type, "Client", "Call set method", $"{Logger.ThreadInfo}");

                _client = value;
            } 
        }
        public List<Tuple<int, Item>> OrderItems
        {
            get
            {
                Logger.Info(Logger.info, type, "List<Tuple<int, Item>>", "Call get method", $"{Logger.ThreadInfo}");

                return _orderItems;
            }
            set
            {
                _orderItems = value;

                Logger.Info(Logger.info, type, "List<Tuple<int, Item>>.ToString()", "Call set method", $"{Logger.ThreadInfo}");

            }
        }
        public string CardNumber
        {
            get
            {
                Logger.Info(Logger.info, type, "CardNumber", "Call get method", $"{Logger.ThreadInfo}");

                return _cardNumber;
            }
            set
            {
                _cardNumber = value;

                Logger.Info(Logger.info, type, "CardNumber", "Call set method", $"{Logger.ThreadInfo}");
            }
        }
        public bool IsCompleted
        {
            get
            {
                Logger.Info(Logger.info, type, "IsCompleted", "Call get method", $"{Logger.ThreadInfo}");

                return (bool)_isCompleted;
            }
            set
            {
                _isCompleted = value;

                Logger.Info(Logger.info, type, "IsCompleted", "Call set method", $"{Logger.ThreadInfo}");
            }
        }
        public bool IsDelivered
        {
            get
            {
                Logger.Info(Logger.info, type, "IsDelivered", "Call get method", $"{Logger.ThreadInfo}");

                return (bool)_isDelivered;
            }
            set
            {
                _isDelivered = value;

                Logger.Info(Logger.info, type, "IsDelivered", "Call set method", $"{Logger.ThreadInfo}");
            }
        }
        public bool IsPaid
        {
            get
            {
                Logger.Info(Logger.info, type, "IsDelivered", "Call get method", $"{Logger.ThreadInfo}");

                return (bool)_isPaid;
            }
            set
            {
                _isPaid = value;
                Logger.Info(Logger.info, type, "IsDelivered", "Call set method", $"{Logger.ThreadInfo}");

            }
        }
        public static int CounterOrder { get; set; } = 1;

        public Order()
        {

        }
        public Order(Client client)
        {
            Id = CounterOrder;
            OrderItems = new List<Tuple<int, Item>>();
            Client = client;
            CounterOrder++;

            Logger.Info(Logger.info, type,"Order", "Call constructor", $"{Logger.ThreadInfo}");

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

            Logger.Info(Logger.info, type, "ShowOrderItems", "Call method", $"{Logger.ThreadInfo}");

            return sumOfAllPositions;
        }

        public bool ToPackOrder(bool prop)
        {
            if (prop)
            {
                IsCompleted = prop;
            }
            Console.WriteLine($"Ваш заказ будет скомплектован в течение 5 минут");
            OrderIsPacked?.Invoke(this, new OrderEventsArgs("Заказ укомплектован", IsCompleted));

            Logger.Info(Logger.info, type,"ToPackOrder", "Call method", $"{Logger.ThreadInfo}");

            return IsCompleted;
        }
        
        public bool ToDeliver(bool prop)
        {
            
            if (prop)
            {
                IsDelivered = prop;
            }
            Console.WriteLine($"Ваш заказ будет доставлен в течении до {DateTime.Now/*.AddHours(2)*/}");
            OrderIsDelivered?.Invoke(this, new OrderEventsArgs("Заказ доставлен", IsDelivered));

            Logger.Info(Logger.info, type,"ToDeliver", "Call method", $"{Logger.ThreadInfo}");

            return IsDelivered; 
        }

        public bool ToPay(bool prop)
        {

            if (prop)
            {
                IsPaid = prop;
            }
            Console.WriteLine($"Ваш заказ оплачен");
            OrderIsPaid?.Invoke(this, new OrderEventsArgs("Заказ оплачен", IsPaid));

            Logger.Info(Logger.info, type, "ToPay", "Call method", $"{Logger.ThreadInfo}");

            return IsPaid;
        }
    }
}