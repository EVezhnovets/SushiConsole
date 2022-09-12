using SushiConsoleDev.Models;
using SushiConsoleDev.Logger;

namespace SushiConsoleDev.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new List<Order>();
        public void CreateOrder(Order order)
        {
            _orders.Add(order);
            Logger.Logger.Info(typeof(OrderRepository), nameof(CreateOrder), "Call method");

        }

        public List<Order> GetAllOrders()
        {
            var allOrders = _orders.ToList();

            Logger.Logger.Info(typeof(OrderRepository), nameof(GetAllOrders), "Call method");

            return allOrders;
        }

        public Order? GetOrderById(int id)
        {
            var orderToGet = _orders.FirstOrDefault(o => o.Id == id);
            Logger.Logger.Info(typeof(OrderRepository), nameof(GetOrderById), "Call method");

            return orderToGet;
        }
        public void ClearList()
        {
            _orders.Clear();
        }
    }
}