using Newtonsoft.Json;
using SushiConsole.Models;
using SushiConsole.Repositories;

ClientRepository clientRepository = new ClientRepository();
OrderRepository orderRepository = new OrderRepository();

while (true)
{
    Client client = new Client();
    Console.WriteLine("Добрый день! Как вас зовут?");
    client.Name = Console.ReadLine();

    Console.WriteLine("Введите свой email");
    client.Email = Console.ReadLine();

    Console.WriteLine("Введите адрес доставки");
    client.Address = Console.ReadLine();

    clientRepository.CreateClient(client);
    //clientRepository.GetAllClients();
    Console.WriteLine();

    Console.WriteLine($"{client.Name}, выберите из списка роллы, которые хотите заказать");

    string pathItems = @"E:\IT\Repositories\SushiConsole\SushiConsole\SushiConsole\Items.json";
    using (StreamReader reader = new StreamReader(pathItems))
    {
        string items = await reader.ReadToEndAsync();

        List<Item> itemsCollection = JsonConvert.DeserializeObject<List<Item>>(items);
        foreach (var item in itemsCollection)
        {
            Console.WriteLine($"Item {item.Id}. {item.Name}, description:{item.Description}, price for 1pc:{item.Price.ToString()}");
        }

        Console.WriteLine();

        Order order = new Order(client);
      
        Console.WriteLine("Для выбора нужной позиции введите номер Item позиции");
        var itemTemp = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Введите желаемое количество ролл");
        var qtyTemp = Convert.ToInt32(Console.ReadLine());

        var itemOrder = itemsCollection.FirstOrDefault(c => c.Id == itemTemp);
        order.OrderItems.Add(qtyTemp, itemOrder);
       
        
        
        Console.WriteLine($"{client.Name}, вы заказали {itemOrder.Name} - {qtyTemp} штук.");
        Console.WriteLine($"Сумма вашего заказа: {itemOrder.Price * qtyTemp} byn");

        order.OrderIsPacked += Order_OrderIsPacked;
        order.OrderIsDelivered += Order_OrderIsDelivered;
        order.OrderIsPaid += Order_OrderIsPaid;

        order.ToPackOrder(true, new TimeSpan( 0, 0, 10));

        Console.WriteLine("Выберите оплату : 1 - оплата картой сейчас\n2 - оплата картой при получении\n3 - оплата наличными при получении");

    }
}

void Order_OrderIsPaid(object arg1, EventArgs arg2)
{
    Console.WriteLine("Ваш заказ оплачен");
}

void Order_OrderIsDelivered(object arg1, EventArgs arg2)
{
    Console.WriteLine("Ваш заказ доставлен");
}

void Order_OrderIsPacked(object arg1, EventArgs arg2)
{
    Console.WriteLine($"Ваш заказ скомплектован");
}



#region Write in file
//string json = JsonConvert.SerializeObject(collectionItems);

//string path = @"E:\IT\Repositories\SushiConsole\SushiConsole\SushiConsole\Items.json";

//using (StreamWriter writer = new StreamWriter(path, false))
//{
//    await writer.WriteLineAsync(json);
//}
#endregion

//Console.WriteLine(json);

//List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);

//foreach (var item in items)
//{
//    Console.WriteLine($"{item.Name}, price: {item.Price}");
//}