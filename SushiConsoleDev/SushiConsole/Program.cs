using Newtonsoft.Json;
using SushiConsole.Models;
using SushiConsole.Repositories;
using SushiConsoleDev.Logger;
using System.Text.RegularExpressions;

ClientRepository clientRepository = new ClientRepository();
OrderRepository orderRepository = new OrderRepository();
bool b = true;

while (b)
{
    //Console.WriteLine(_dateNameFile);
    //Logger.Info();
    //Logger.Debug();
    //Logger.Error();

    Client client = new Client();
    Console.WriteLine("Добрый день! Вас приветсвует мини-бот по доставке суши. Как вас зовут?");
    //to make Validation
    client.Name = Console.ReadLine();
    //Logger.Info(client);

    Console.WriteLine(client.Id);

    Console.WriteLine("Введите свой email");
    //to make Validation
    client.Email = Console.ReadLine();

    Console.WriteLine("Введите адрес доставки");
    //to make Validation
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
        Item itemOrder = default;
        int qtyTemp = default;
        bool b1 = true;
        while (b1)
        {
            Console.WriteLine("Для выбора нужной позиции введите номер Item позиции");
            var itemTemp = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Введите желаемое количество ролл");
            qtyTemp = Convert.ToInt32(Console.ReadLine());

            itemOrder = itemsCollection.FirstOrDefault(c => c.Id == itemTemp);
            order.OrderItems.Add(new Tuple<int, Item>(qtyTemp, itemOrder));

            Console.WriteLine("Желаете выбрать еще роллы? Введите 1 - да, хочу еще выбрать   2 - нет, завершить заказ");

            bool b2 = true;
            while (b2)
            {
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        b2 = false;
                        break;
                    case "2":
                        b1 = false;
                        b2 = false;
                        break;
                    default:
                        Console.WriteLine("Введите корректное значение 1 - выбрать роллы снова, 2 - завершить заказ");
                        break;
                }

            }
        }

        Console.WriteLine($"{client.Name}, вы заказали "/*{itemOrder.Name} - {qtyTemp} штук."*/);
        var sum = order.ShowOrderItems();
        Console.WriteLine($"Сумма вашего заказа: {sum} byn");

        order.OrderIsPacked += Order_OrderIsPacked;
        order.OrderIsDelivered += Order_OrderIsDelivered;
        order.OrderIsPaid += Order_OrderIsPaid;
        
        bool b3 = true;
        while (b3)
        {
            Console.WriteLine("Выберите оплату : 1 - оплата картой сейчас\n2 - оплата картой или наличными при получении");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    bool b4 = true;
                    while (b4)
                    {
                        Console.WriteLine("Введите данные карты в формате xxxx xxxx xxxx xxxx");
                        order.CardNumber = Console.ReadLine();
                        string cardNumber = @"\d{4}\s\d{4}\s\d{4}\s\d{4}";
                        if (Regex.IsMatch(order.CardNumber, cardNumber))
                        {

                            b4 = false;
                        }
                        else
                        {
                            Console.WriteLine("Validation failed. Ввелите ваоидный номер карты"); ;
                        }
                    }
                    b4 = true;
                    while (b4)
                    {
                        Console.WriteLine("Введите месяц/год карты в формате xx xx");
                        order.CardNumber = Console.ReadLine();
                        string yearMonth = @"\d{2}\s\d{2}";
                        if (Regex.IsMatch(order.CardNumber, yearMonth))
                        {

                            b4 = false;
                        }
                        else
                        {
                            Console.WriteLine("Validation failed. Ввелите валидный номер карты"); ;
                        }
                    }
                    b3 = false;
                    break;
                case "2":
                    b3 = false;
                    break;
                default:
                    Console.WriteLine("Введите корректное значение");
                    break;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"{client.Name}, Ваш заказ:\nНомер: {order.Id}\n{itemOrder.Name} - {qtyTemp} штук.");
        Console.WriteLine($"Сумма вашего заказа: {itemOrder.Price * qtyTemp} byn");
        Console.WriteLine($"\nПосле получения уведомления о статусе заказа, осуществится доставка по адресу {client.Address} в течении 30 мин");
        Console.WriteLine("Хорошего Вам дня!");

        Console.WriteLine();
        Console.WriteLine("Сделать новый заказ? Введите 1 - да, 2 - нет");
        
        bool b5 = true;
        while (b5)
        {
            var input2 = Console.ReadLine();
            switch (input2)
            {
                case "1":
                    b5 = false;
                    break;
                case "2":
                    b5 = false;
                    b = false;
                    break;
                default:
                    Console.WriteLine("Введите корректное значение: 1 - продолжить, 2 - выйти");
                    break;
            }
        }
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