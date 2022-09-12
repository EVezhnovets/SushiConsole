using SushiConsoleDev.Models;
using SushiConsoleDev.Repositories;
using SushiConsoleDev.Email;
using SushiConsoleDev.HappyBirthday;
using System.Text.RegularExpressions;
using SushiConsoleDev.Views;
using Spectre.Console;
using SushiConsoleDev.Extensions;

ClientRepository clientRepository = new ClientRepository();
OrderRepository orderRepository = new OrderRepository();
ItemRepository itemRepository = new ItemRepository();
Client client = new Client();
Order order = new Order(client);

AnsiConsole.Markup("[yellow]Hi! Welcome to the SushiStore mini - bot for sushi delivery.[/]");

order.OrderIsPacked += Order_OrderIsPacked;
order.OrderIsDelivered += Order_OrderIsDelivered;
order.OrderIsPaid += Order_OrderIsPaid;

bool isOneMoreOrder = true;
while (isOneMoreOrder)
{
    bool isNameValid = true;
    while (isNameValid)
    {
        isNameValid = InputName(isNameValid);
    }

    clientRepository.CreateClient(client);
    Console.WriteLine();

    AnsiConsole.MarkupLine($"[yellow2]{client.Name}[/], [yellow]Select the rolls you want to order from the list (price for 1 pc)[/]");

    var itemsCollection = await itemRepository.GetItemColloection();
    ItemView.ShowItemsCollection(itemsCollection);

    Console.WriteLine();

    Item? itemOrder = default;
    int qtyClientChoise = default;
    int itemClientChoiсe = default;

    bool isOrderInputValid = true;
    while (isOrderInputValid)
    {
        bool isItemInputValid = true;
        while (isItemInputValid)
        {
            isItemInputValid = InputItem(isItemInputValid, itemsCollection, out itemClientChoiсe);
        }

        bool isQtyInputValid = true;
        while (isQtyInputValid)
        {
            isQtyInputValid = InputQty(isQtyInputValid, out qtyClientChoise);
        }

        itemOrder = itemsCollection.FirstOrDefault(c => c.Id == itemClientChoiсe);
        order.OrderItems.Add(new Tuple<int, Item>(qtyClientChoise, itemOrder));

        AnsiConsole.MarkupLine("[yellow2]Would you like more rolls?[/] [yellow]Enter [yellow2]1[/] - Yes, I want to choose more\n[yellow2]2[/] - No, complete the order\n[yellow2]3[/] - Reset, start the order selection again[/]");

        bool isNewOrderValid = true;
        while (isNewOrderValid)
        {
            isNewOrderValid = IsContinueOrder(isNewOrderValid, out isOrderInputValid);
        }
    }

    Console.WriteLine();
    HorizontalRule("Pre order");
    
    order.ShowOrderItems(order,out string summaryOrder);
    AnsiConsole.MarkupLine($"[yellow][yellow2]{client.Name}[/], your order:[/]\n{summaryOrder}");

    HBCheck(order);

    bool isValidInputEmail = true;
    while (isValidInputEmail)
    {
        isValidInputEmail = InputEmail(isValidInputEmail);
    }
    order.ToPackOrder();

    Address address = new Address();
    client.Address = address;

    AnsiConsole.MarkupLine("[yellow]Enter shipping address[/]");

    bool isInputStreetValid = true;
    while (isInputStreetValid)
    {
        isInputStreetValid = InputAddressStreet(isInputStreetValid, address);
    }

    AnsiConsole.MarkupLine("[yellow]Enter house number[/]");
    bool isInputHouseNumValid = true;
    while (isInputHouseNumValid)
    {
        isInputHouseNumValid = InputAddressHouseNumber(isInputHouseNumValid, address);
    }

    AnsiConsole.MarkupLine("[yellow]Enter building (if missing, enter NO)[/]");
    bool isInputBuildingValid = true;
    while (isInputBuildingValid)
    {
        isInputBuildingValid = InputAddressBuilding(isInputBuildingValid, address);
    }

    AnsiConsole.MarkupLine("[yellow]Enter apartment number (if missing, enter NO)[/]");
    bool isInputApartmentNumValid = true;
    while (isInputApartmentNumValid)
    {
        isInputApartmentNumValid = InputAddressApartmentNumber(isInputApartmentNumValid, address);
    }

    AnsiConsole.MarkupLine("[yellow]Choose payment: [yellow2]1[/] - payment by card online\n[yellow2]2[/] - payment by card or cash upon receipt[/]");
    bool isInputPaymentChoiceValid = true;
    while (isInputPaymentChoiceValid)
    {
        var inputPaymentChoiseNumber = Console.ReadLine();
        switch (inputPaymentChoiseNumber)
        {
            case "1":
                bool isValidCardNumber = true;
                while (isValidCardNumber)
                {
                    isValidCardNumber = InputCardNumber(isValidCardNumber);
                }
                bool isValidCardMonthAndNumber = true;
                while (isValidCardMonthAndNumber)
                {
                    isValidCardMonthAndNumber = InputCardMonthNumber(isValidCardMonthAndNumber);
                }
                isInputPaymentChoiceValid = false;
                order.ToPayOrder();
                break;
            case "2":
                AnsiConsole.MarkupLine("[yellow]Payment will be made after delivery[/]");
                isInputPaymentChoiceValid = false;
                break;
            default:
                AnsiConsole.MarkupLine("[red]Please enter a valid value. 1 - payment by card online\n2 - payment by card or cash upon receipt[/]");
                break;
        }
    }

    Console.WriteLine();

    order.ShowOrderItems(order, out string newSummaryOrder);
    var border = Order.CreatePanel(newSummaryOrder, BoxBorder.Double, client);

    AnsiConsole.Write(new Padder(new Columns(border).PadRight(2), new Padding(2, 0, 0, 0)));
    ShowNotice(order, client);

    order.ToDeliverOrder();

    Console.WriteLine();

    AnsiConsole.MarkupLine("[yellow]Do you want to use the bot again? Enter [yellow2]1[/] - yes, [yellow2]2[/] - no[/]");
    bool isReloadInputNumberValid = true;
    while (isReloadInputNumberValid)
    {
        isReloadInputNumberValid = IsContinueBotUsing(isReloadInputNumberValid, out isOneMoreOrder);
        order.OrderItems.Clear();
    }
}
AnsiConsole.MarkupLine("[green1]Have a nice day![/]");

Console.ReadKey();

void Order_OrderIsPaid(object arg1, EventArgs arg2)
{
    AnsiConsole.MarkupLine("[invert yellow3_1]Your order has been paid[/]");
    Task<bool> task = EmailSender.SendEmail(client.Email, "Hello, I'm SushiStore.",
                $"{client.Name}, your order #{order.Id} amount {OrderExtensions.ShowOrderPrice(order)} byn has been paid.");
}

void Order_OrderIsDelivered(object arg1, EventArgs arg2)
{
    AnsiConsole.MarkupLine("[invert yellow3_1]Your order will be delivered within 2 hours[/]");
    Task<bool> task = EmailSender.SendEmail(client.Email, "Hello, I'm SushiStore.",
                $"{client.Name}, your order #{order.Id} amount {OrderExtensions.ShowOrderPrice(order)} byn has been delivered until {DateTime.Now.AddHours(2)}.");
}

void Order_OrderIsPacked(object arg1, EventArgs arg2)
{
    AnsiConsole.MarkupLine("[invert yellow3_1]Your order will be completed within 5 minutes[/]");
    Task<bool> task = EmailSender.SendEmail(client.Email, "Hello, I'm SushiStore",
                $"{client.Name}, your order #{order.Id} на сумму {OrderExtensions.ShowOrderPrice(order)} byn is complete.");
}

bool isValidEmailInput(string email)
{
    string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
    Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
    return isMatch.Success;
}

bool InputName(bool isNameValid)
{
    AnsiConsole.Markup("[yellow2] What is your name?[/] ");
    var input1 = Console.ReadLine();

    string pattern = "^[a-zA-Zа-яА-Я]+$";
    Match isMatch = Regex.Match(input1, pattern);
    if (!String.IsNullOrWhiteSpace(input1) && input1.Length > 1 && input1.Length < 20 && isMatch.Success)
    {
        input1 = input1.Trim();
        client.Name = input1;
        isNameValid = false;
    }
    else
    {
        AnsiConsole.Markup("[red]The value must not be empty or filled with spaces, " +
            "and must be between 2 and 20 characters.\nEnter a valid value[/]");
    }
    return isNameValid;
}

bool InputItem(bool isItemInputValid, List<Item> itemsCollection, out int itemClientChoiсe)
{
    AnsiConsole.MarkupLine($"[yellow]To select the desired item, enter the Item number[/]");
    var itemTemp = Console.ReadLine();

    if (int.TryParse(itemTemp, out itemClientChoiсe))
    {
        if (itemClientChoiсe > 0 && itemClientChoiсe <= itemsCollection.Count)
        {
            itemClientChoiсe = Convert.ToInt32(itemTemp);
            isItemInputValid = false;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]The value must not be empty or filled with spaces, must be a number between 1 and {itemsCollection.Count}.\nEnter a valid value[/]");
        }
    }
    else
    {
        AnsiConsole.MarkupLine($"[red]The value must not be empty or filled with spaces, must be a number between 1 and {itemsCollection.Count}.\nEnter a valid value[/]");
    }
    return isItemInputValid;
}

bool InputQty(bool isQtyInputValid, out int qtyClientChoise)
{
    AnsiConsole.MarkupLine("[yellow]Enter the desired number of rolls[/]");
    var qtyTemp = Console.ReadLine();

    if (int.TryParse(qtyTemp, out qtyClientChoise))
    {
        if (qtyClientChoise > 0 && qtyClientChoise <= Order.maxNumberOfRollsPerOrder)
        {
            qtyClientChoise = Convert.ToInt32(qtyTemp);
            isQtyInputValid = false;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]The value must not be empty or filled with spaces, must be a number between 1 and {Order.maxNumberOfRollsPerOrder}.\nEnter a valid value[/]");
        }
    }
    else
    {
        AnsiConsole.MarkupLine($"[red]The value must not be empty or filled with spaces, must be a number between 1 and {Order.maxNumberOfRollsPerOrder}.\nEnter a valid value[/]");
    }
    return isQtyInputValid;
}

bool IsContinueOrder(bool isNewOrderValid, out bool isOrderInputValid)
{
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            isNewOrderValid = false;
            isOrderInputValid = true;
            return isNewOrderValid;
            break;
        case "2":
            isOrderInputValid = false;
            isNewOrderValid = false;
            return isNewOrderValid;
            break;
        case "3":
            order.OrderItems.Clear();
            isNewOrderValid = false;
            isOrderInputValid = true;
            return isNewOrderValid;
            break;
        default:
            AnsiConsole.MarkupLine($"[red]Enter the correct value: 1 - Select rolls again 2 - Complete the order 3 - Reset, start order selection again[/]");
            isOrderInputValid = true;
            return isNewOrderValid;
            break;
    }
}

bool InputEmail(bool isValidInputEmail)
{
    AnsiConsole.MarkupLine("[yellow]Enter your email[/]");
    var emailInput = Console.ReadLine();
    bool isValidEmailInputResult = isValidEmailInput(emailInput);
    if (isValidEmailInputResult)
    {
        client.Email = emailInput;
        isValidInputEmail = false;
    }
    else
    {
        AnsiConsole.MarkupLine("[red]Enter valid email[/]");
    }
    return isValidInputEmail;
}

bool InputAddressStreet(bool isInputStreetValid, Address address)
{
    AnsiConsole.MarkupLine("[yellow]Enter the street[/]");
    string inputStreetValid = Console.ReadLine();
    if (!String.IsNullOrWhiteSpace(inputStreetValid) && inputStreetValid.Length >= 3 && inputStreetValid.Length < 20)
    {
        address.Street = inputStreetValid;
        isInputStreetValid = false;
    }
    else
    {
        AnsiConsole.MarkupLine("[red]The value must not be empty or filled with spaces and must be between 3 and 20 characters." +
            "\nEnter a valid value[/]");
    }
    return isInputStreetValid;
}

bool InputAddressHouseNumber(bool isInputHouseNumValid, Address address)
{
    string inputHouseNum = Console.ReadLine();
    var IsInputParsed = int.TryParse(inputHouseNum, out int houseNum);
    if (IsInputParsed && houseNum < 999)
    {
        address.NumberOfHouse = houseNum;
        isInputHouseNumValid = false;
    }
    else
    {
        AnsiConsole.MarkupLine("[red]Value must be an integer\nPlease enter a valid value[/]");
    }
    return isInputHouseNumValid;
}

bool InputAddressBuilding(bool isInputBuildingValid, Address address)
{
    string inputBuildingValid = Console.ReadLine();

    if (inputBuildingValid.ToLower() == "no")
    {
        address.Building = inputBuildingValid;
        isInputBuildingValid = false;
    }
    else if (!String.IsNullOrWhiteSpace(inputBuildingValid) && inputBuildingValid.Length >= 1 && inputBuildingValid.Length < 3)
    {
        address.Building = inputBuildingValid;
        isInputBuildingValid = false;
    }
    else
    {
        AnsiConsole.MarkupLine("[red]Value must be an integer or NO word\nPlease enter a valid value[/]");
    }
    return isInputBuildingValid;
}

bool InputAddressApartmentNumber(bool isInputApartmentNumValid, Address address)
{
    string inputApartmentNum = Console.ReadLine();
    var IsInputParsed = int.TryParse(inputApartmentNum, out int ApartmentNum);
    if (inputApartmentNum.ToLower() == "no")
    {
        address.NumberOfApartment = inputApartmentNum;
        isInputApartmentNumValid = false;
    }
    else if (IsInputParsed && ApartmentNum < 1200)
    {
        address.NumberOfApartment = ApartmentNum.ToString();
        isInputApartmentNumValid = false;
    }
    else
    {
        AnsiConsole.MarkupLine("[red]Please enter a valid value[/]");
    }
    return isInputApartmentNumValid;
}

bool InputCardNumber(bool isValidCardNumber)
{
    AnsiConsole.MarkupLine("[yellow]Enter your card details in the format [white]xxxx xxxx xxxx xxxx[/][/]");
    order.CardNumber = Console.ReadLine();
    string cardNumber = @"\d{4}\s\d{4}\s\d{4}\s\d{4}";
    if (Regex.IsMatch(order.CardNumber, cardNumber))
    {

        isValidCardNumber = false;
    }
    else
    {
        AnsiConsole.MarkupLine("[red]Validation failed. Please enter a valid card number[/]"); ;
    }
    return isValidCardNumber;
}

bool InputCardMonthNumber(bool isValidCardMonthAndNumber)
{
    bool isInputYearValid = true;
    int inputYearNumber = default;
    while (isInputYearValid)
    {
        AnsiConsole.MarkupLine($"[yellow]Enter the card expiry year[/]");
        var inputYear = Console.ReadLine();
        bool isInputYearNumberValid = int.TryParse(inputYear, out inputYearNumber);

        if (isInputYearValid && inputYearNumber >= DateTime.Now.Year && inputYearNumber <= DateTime.Now.AddYears(50).Year)
        {
            isInputYearValid = false;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Enter valid value from {DateTime.Now.Year} to {DateTime.Now.AddYears(50).Year}[/]");
        }
    }

    bool isInputMonthValid = true;
    int inputMonthNumber = default;
    while (isInputMonthValid)
    {
        AnsiConsole.MarkupLine($"[yellow]Enter the month number of your card[/]");
        var inputMonth = Console.ReadLine();
        bool isInputMonthNumberOfBirthValid = int.TryParse(inputMonth, out inputMonthNumber);
        if(inputYearNumber == DateTime.Now.Year)
        {
            if(DateTime.Now.Month <= inputMonthNumber && inputMonthNumber <= 12)
            {
                isInputMonthValid = false;
                isValidCardMonthAndNumber = false;
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Enter valid value from {DateTime.Now.Month} to 12[/]");
                continue;
            }
        } else if (isInputMonthNumberOfBirthValid && inputMonthNumber > 0 && inputMonthNumber <= 12)
        {
            isInputMonthValid = false;
            isValidCardMonthAndNumber = false;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Enter valid value from 1 to 12[/]");
        }
    }
    return isValidCardMonthAndNumber;
}

bool IsContinueBotUsing(bool isReloadInputNumberValid, out bool isOneMoreOrder)
{
    var inputReloadNumber = Console.ReadLine();
    switch (inputReloadNumber)
    {
        case "1":
            isReloadInputNumberValid = false;
            isOneMoreOrder = true;
            return isReloadInputNumberValid;
            break;
        case "2":
            isReloadInputNumberValid = false;
            isOneMoreOrder = false;
            return isReloadInputNumberValid;
            break;
        default:
            AnsiConsole.MarkupLine("[red]Please enter a valid value: 1 - continue, 2 - exit[/]");
            isOneMoreOrder = true;
            return isReloadInputNumberValid;
            break;
    }
}

void ShowNotice(Order order, Client client)
{
    AnsiConsole.MarkupLine($"[yellow]\nAfter receiving notification of the status of the order, delivery will be made to the address: " +
        $"street [yellow2]{client.Address.Street}[/], house [yellow2]{client.Address.NumberOfHouse}[/], building [yellow2]{client.Address.Building}[/], " +
        $"apartment [yellow2]{client.Address.NumberOfApartment}[/] up to [invert magenta2]{DateTime.Now.AddHours(2)}[/][/]");
}

void HorizontalRule(string title)
{
    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule($"[white bold]{title}[/]").RuleStyle("grey").LeftAligned());
    AnsiConsole.WriteLine();
}

void HBCheck(Order order)
{
    bool isInputMonthValid = true;
    int inputMonthNumber = default;
    int inputDayNumber = default;

    while (isInputMonthValid)
    {
        AnsiConsole.MarkupLine($"[yellow]Enter the month number of your birth[/]");
        var inputMonthNumberOfBirth = Console.ReadLine();
        bool isInputMonthNumberOfBirthValid = int.TryParse(inputMonthNumberOfBirth, out inputMonthNumber);

        if (isInputMonthNumberOfBirthValid && inputMonthNumber > 0 && inputMonthNumber <= 12)
        {
            isInputMonthValid = false;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Enter valid value from 1 to 12[/]");
        }
    }

    bool isInputDayValid = true;
    while (isInputDayValid)
    {
        AnsiConsole.MarkupLine($"[yellow]Input the day of your birth[/]");
        var inputDayOfBirth = Console.ReadLine();
        bool isInputDayOfBirthValid = int.TryParse(inputDayOfBirth, out inputDayNumber);
        switch (inputMonthNumber)
        {
            case 1 or 3 or 5 or 7 or 8 or 10 or 12:
                if (isInputDayOfBirthValid && inputDayNumber > 0 && inputDayNumber <= 31)
                {
                    isInputDayValid = false;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Enter valid value from 1 to 31[/]");
                }
                break;
            case 4 or 6 or 9 or 11:
                if (isInputDayOfBirthValid && inputDayNumber > 0 && inputDayNumber <= 30)
                {
                    isInputDayValid = false;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Enter valid value from 1 to 30[/]");
                }
                break;
            default:
                if (isInputDayOfBirthValid && inputDayNumber > 0 && inputDayNumber <= 29)
                {
                    isInputDayValid = false;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Enter valid value from 1 to 29[/]");
                }
                break;
        }
    }
    if (inputDayNumber == DateTime.Now.Day && inputMonthNumber == DateTime.Now.Month)
    {
        order.Koef = 0.6m;
        AnsiConsole.MarkupLine($"[invert seagreen1]Happy Birthday! This song from the bottom of my transistor`s heart is for you![/]");
        AnsiConsole.MarkupLine("[yellow]Do nothing while the melody plays, enjoy it[/]");
        HappyBirthdayCongratulator.ToCongratulate();
        AnsiConsole.MarkupLine($"[yellow2]{order.Client.Name}[/][yellow], on your birthday you have a [invert yellow2]40%[/] discount on the total amount of the order, no thanks[/]");
    }
}
#region CycleCheckForCreateLoggerFile
//Console.WriteLine("Зацикливаюсь!");
//Thread.Sleep(100);
//Client client = new Client();
#endregion