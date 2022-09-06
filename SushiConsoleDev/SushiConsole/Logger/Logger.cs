using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsoleDev.Logger
{
    public class Logger
    {
        public static string _dateTime = String.Format("{0:yyyyMMdd}_", DateTime.UtcNow);
        public static string _counter = "counter";
        public static string path = @$"E:\IT\Repositories\SushiConsoleDev\SushiConsoleDev\SushiConsole\Logger\LoggerRepository\log{_dateTime}[{_counter}].txt";
        public readonly int ThreadInfo = Thread.CurrentThread.ManagedThreadId;
        public static void Info(Type type, string method, string message, string threadInfo)
        {
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                StringBuilder sb = new StringBuilder();
                var time = String.Format("{0:HH:mm:ss dd/MM/yyyy}_", DateTime.Now);
                var fromNamespaceType = type.ToString();
                sb.Append(time);
                sb.Append($"{fromNamespaceType}" + $".{method}_");
                sb.Append($"{message}_");
                sb.Append(threadInfo);
                sb.ToString();
                streamWriter.WriteLine(sb.ToString());

            }
            
        }

        public static void Debug()
        {
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                streamWriter.WriteLine("Hello from Logger.Debug");
            }
        }
     
        public static void Error()
        {
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                streamWriter.WriteLine("Hello from Logger.Error");
            }
        }

        public void CheckFileSize()
        {

        }
    }
}
