using Spectre.Console;
using Spectre.Console.Rendering;
using SushiConsoleDev.Email;
using SushiConsoleDev.Logger;
using System.Text;

namespace SushiConsoleDev.Models
{
    public class Order
    {
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

        public int Id { get; set; }
        public Client Client { get; set; }
        public List<Tuple<int, Item>> OrderItems { get; set; }
        public string CardNumber { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsPaid { get; set; }
        public decimal Koef { get; set; } = 1.0m;
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
        }

        public void ShowOrderItems(Order order, out string summaryOrder)
        {
            List<decimal> sumOfPositionList = new List<decimal>();
            decimal sumOfPosition = default;
            decimal sumOfAllPositions = default;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in OrderItems)
            {
                stringBuilder.Append($"[green][yellow3]{item.Item2.Name}[/]: {item.Item1} pc - {sumOfPosition = item.Item2.Price * item.Item1} byn[/]\n");
                sumOfPositionList.Add(sumOfPosition);
            }
            foreach (var item in sumOfPositionList)
            {
                sumOfAllPositions += item;
            }

            string lastLine = $"[yellow]Your order amount: [invert yellow3]{sumOfAllPositions * order.Koef}[/] byn[/]";
            stringBuilder.Append(lastLine);
            summaryOrder = stringBuilder.ToString();

            Logger.Logger.Info(typeof(Order), nameof(ShowOrderItems), "Call method");
        }

        public decimal ShowOrderPrice(decimal koef)
        {
            List<decimal> sumList = new List<decimal>();
            decimal sumPosition = default;
            decimal sumAllPositions = default;
            foreach (var item in OrderItems)
            {
                sumPosition = item.Item2.Price * item.Item1;
                sumList.Add(sumPosition);
            }
            foreach (var item in sumList)
            {
                sumAllPositions += item;
            }
            Logger.Logger.Info(typeof(Order), nameof(ShowOrderPrice), "Call method");
            return sumAllPositions * koef;
        }

        public bool ToPackOrder()
        {
            OrderIsPacked?.Invoke(this, new OrderEventsArgs("The order is completed", IsCompleted));
           
            Logger.Logger.Info(typeof(Order), nameof(ToPackOrder), "Call method");

            return IsCompleted = true;
        }
        
        public bool ToDeliverOrder()
        {
            OrderIsDelivered?.Invoke(this, new OrderEventsArgs("The order is delivered", IsDelivered));

            Logger.Logger.Info(typeof(Order), nameof(ToDeliverOrder), "Call method");

            return IsDelivered = true; 
        }

        public bool ToPayOrder()
        {
            OrderIsPaid?.Invoke(this, new OrderEventsArgs("Order has been paid", IsPaid));

            Logger.Logger.Info(typeof(Order), nameof(ToPayOrder), "Call method");

            return IsPaid = true;
        }

        public static IRenderable CreatePanel(string summaryOrder, BoxBorder border, Client client)
        {
            return new Panel(summaryOrder)
                .Header($"[yellow2]{client.Name}[/],[yellow] you ordered[/] ", Justify.Center)
                .Border(border)
                .BorderStyle(Style.Parse("grey"));
        }
    }
}