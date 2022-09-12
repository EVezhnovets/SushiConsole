using SushiConsoleDev.Models;

namespace SushiConsoleDev.Extensions
{
    public static class OrderExtensions
    {
        public static decimal ShowOrderPrice(Order order)
        {
            List<decimal> sumList = new List<decimal>();
            decimal sumPosition = default;
            decimal sumAllPositions = default;
            foreach (var item in order.OrderItems)
            {
                sumPosition = item.Item2.Price * item.Item1;
                sumList.Add(sumPosition);
            }
            foreach (var item in sumList)
            {
                sumAllPositions += item;
            }
            Logger.Logger.Info(typeof(OrderExtensions), nameof(ShowOrderPrice), "Call method");
            var result = sumAllPositions * order.Koef;
            return result;
        }
    }
}
