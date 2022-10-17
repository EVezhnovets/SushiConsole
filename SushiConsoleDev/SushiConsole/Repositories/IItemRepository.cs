using SushiConsoleDev.Models;

namespace SushiConsoleDev.Repositories
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemColloection();
    }
}
