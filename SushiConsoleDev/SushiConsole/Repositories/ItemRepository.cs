using Newtonsoft.Json;
using SushiConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsoleDev.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private string pathItems = @"E:\IT\Repositories\SushiConsole\SushiConsole\SushiConsole\Items.json";

        public async Task<List<Item>> ToMakeItemCollectionFromJson()
        {
            List<Item> itemsCollection = new List<Item>();
            using (StreamReader reader = new StreamReader(pathItems))
            {
                string items = await reader.ReadToEndAsync();

                itemsCollection = JsonConvert.DeserializeObject<List<Item>>(items);
                return itemsCollection;
            }
        }

        public void ShowItemsCollection(List<Item> itemsCollection)
        {
            foreach (var item in itemsCollection)
            {
                Console.WriteLine($"Item {item.Id}. {item.Name}, description:{item.Description}, price for 1pc:{item.Price.ToString()}");
            }
        }
    }
}
