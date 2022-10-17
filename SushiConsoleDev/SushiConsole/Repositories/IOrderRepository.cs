using SushiConsoleDev.Models;

namespace SushiConsoleDev.Repositories
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
    }
}