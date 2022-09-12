namespace SushiConsoleDev.Models
{
    public class OrderEventsArgs : EventArgs
    {
        public string Message { get; set; }
        public bool BoolArg { get; set; }

        public OrderEventsArgs(string message, bool boolArg)
        {
            Message = message;
            BoolArg = boolArg;
        }
    }
}