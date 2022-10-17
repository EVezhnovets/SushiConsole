using Newtonsoft.Json;
using SushiConsoleDev.Models;
using SushiConsoleDev.Exceptions;
using SushiConsoleDev.Logger;

namespace SushiConsoleDev.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private string pathItems = @"E:\IT\Repositories\SushiConsole\SushiConsole\SushiConsole\Items.json";

        public async Task<List<Item>> GetItemColloection()
        {
            List<Item> itemsCollection = new List<Item>();
            try
            {
                StreamReader reader = new StreamReader(GetPath());
                string items = await reader.ReadToEndAsync();
                itemsCollection = JsonConvert.DeserializeObject<List<Item>>(items);
                Logger.Logger.Info(typeof(ItemRepository), nameof(GetItemColloection), "Call method");
                return itemsCollection;
            }
            catch (Exception ex)
            {
                throw new JsonNullException("File not found");
            }
        }
        public string GetPath()
        {
            var path = Environment.CurrentDirectory + "\\Items.json";
            return path;
        }
    }
}
