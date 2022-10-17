using SushiConsoleDev.Models;
using SushiConsoleDev.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace SushiConsoleDev.Views
{
    public class ItemView
    {
        public static void ShowItemsCollection(List<Item> itemsCollection)
        {
            foreach (var item in itemsCollection)
            {
                AnsiConsole.MarkupLine($"[yellow]{item.Id}: [white]{item.Name}[/] - [invert italic yellow2]{item.Price}[/] byn[/]");
            }
            Logger.Logger.Info(typeof(ItemView), nameof(ShowItemsCollection), "Call method");
        }
    }
}
