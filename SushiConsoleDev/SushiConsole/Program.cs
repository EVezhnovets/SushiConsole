using Newtonsoft.Json;
using SushiConsole.Models;
using SushiConsole.Repositories;
using SushiConsoleDev.Email;
using SushiConsoleDev.HappyBirthday;
using SushiConsoleDev.Logger;
using SushiConsoleDev.Repositories;
using System.Text.RegularExpressions;

ClientRepository clientRepository = new ClientRepository();
OrderRepository orderRepository = new OrderRepository();
ItemRepository itemRepository = new ItemRepository();
bool b = true;

while (b)
{
    Client client = new Client();
    Order order = new Order(client);
    bool check1 = true;
    Console.WriteLine("Добрый день! Вас приветсвует мини-бот по доставке суши.");
    while (check1)
    {
        Console.WriteLine("Как вас зовут?");
        var input1 = Console.ReadLine();
        if (!String.IsNullOrWhiteSpace(input1) && input1.Length > 3 && input1.Length < 20 )
        {
            client.Name = input1;
            check1 = false;
        }
        else
        {
            Console.WriteLine("Значение не должно быть пустым или заполнено пробелами," +
                "и должно быть в диапазоне от 3 до 20 знаков.\nВведите корректное значение");
        }
    }

    clientRepository.CreateClient(client);
    Console.WriteLine();

    Console.WriteLine($"{client.Name}, выберите из списка роллы, которые хотите заказать");

    var itemsCollection = await itemRepository.ToMakeItemCollectionFromJson();
    itemRepository.ShowItemsCollection(itemsCollection);

    Console.WriteLine();

    Item? itemOrder = default;
    int qtyClientChoise = default;
    int itemClientChoiсe = default;

    bool b1 = true;
    while (b1)
    {
        bool check2 = true;
        while (check2)
        {
            Console.WriteLine("Для выбора нужной позиции введите номер Item позиции");
            var itemTemp = Console.ReadLine();

            if (int.TryParse(itemTemp, out itemClientChoiсe))
            {
                if (itemClientChoiсe > 0 && itemClientChoiсe < itemsCollection.Count)
                {
                    itemClientChoiсe = Convert.ToInt32(itemTemp);
                    check2 = false;
                }
                else
                {
                    Console.WriteLine($"Значение не должно быть пустым или заполнено пробелами, должно быть числом от 1 до {itemsCollection.Count}.\nВведите корректное значение");
                }
            }
            else
            {
                Console.WriteLine($"Значение не должно быть пустым или заполнено пробелами, должно быть числом от 1 до {itemsCollection.Count}.\nВведите корректное значение");
            }
        }

        bool check3 = true;
        while (check3)
        {
            Console.WriteLine("Введите желаемое количество ролл");
            var qtyTemp = Console.ReadLine();

            if (int.TryParse(qtyTemp, out qtyClientChoise))
            {
                if (qtyClientChoise > 0 && qtyClientChoise < Order.maxNumberOfRollsPerOrder)
                {
                    qtyClientChoise = Convert.ToInt32(qtyTemp);
                    check3 = false;
                }
                else
                {
                    Console.WriteLine($"Значение не должно быть пустым или заполнено пробелами, должно быть числом от 1 до {Order.maxNumberOfRollsPerOrder}.\nВведите корректное значение");
                }
            }
            else
            {
                Console.WriteLine($"Значение не должно быть пустым или заполнено пробелами, должно быть числом от 1 до {Order.maxNumberOfRollsPerOrder}.\nВведите корректное значение");
            }
        }

        itemOrder = itemsCollection.FirstOrDefault(c => c.Id == itemClientChoiсe);
        order.OrderItems.Add(new Tuple<int, Item>(qtyClientChoise, itemOrder));
            
        //label
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

    Console.WriteLine("Введите свой email");
    //to make Validation
    client.Email = Console.ReadLine();

    Console.WriteLine("Введите адрес доставки");
    //to make Validation
    client.Address = Console.ReadLine();

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
    Console.WriteLine($"{client.Name}, Ваш заказ:\nНомер: {order.Id}\n{itemOrder.Name} - {qtyClientChoise} штук.");
    Console.WriteLine($"Сумма вашего заказа: {itemOrder.Price * qtyClientChoise} byn");
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

//EmailSender.SendEmail("eugene.vezhnavets@gmail.com", "Привет из теста smtp", "Тест smtp");
#region CycleCheckForCreateLoggerFile
//Console.WriteLine("Зацикливаюсь!");
//Thread.Sleep(100);
//Client client = new Client();
#endregion
#region Write in file
//string json = JsonConvert.SerializeObject(collectionItems);

//string path = @"E:\IT\Repositories\SushiConsole\SushiConsole\SushiConsole\Items.json";

//using (StreamWriter writer = new StreamWriter(path, false))
//{
//    await writer.WriteLineAsync(json);
//}
#endregion