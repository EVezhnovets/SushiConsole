using SushiConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new List<Order>();
        public void CreateOrder(Order order)
        {
            _orders.Add(order);
        }

        public List<Order> GetAllOrders()
        {
            var allOrders = _orders.ToList();

            foreach (var item in allOrders)
            {
                Console.WriteLine($"Order Id :{item.Id}, Client: {item.Client.Name}, address:" +
                    $"{item.Client.Address}, Is Comleted:{item.IsCompleted}, Is Delivered:" +
                    $"{item.IsDelivered}, Is Paid:{item.IsPaid}");
            }

            return allOrders;
        }

        public Order GetOrderById(int id)
        {
            var orderToGet = _orders.FirstOrDefault(o => o.Id == id);
            return orderToGet;
        }
    }
}