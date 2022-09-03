using SushiConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Repositories
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
    }
}