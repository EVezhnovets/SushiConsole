using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsoleDev.Logger
{
    public class Logger
    {
        public static string info = "INFO";
        public static string debug = "DEBUG";
        public static string error = "ERROR";

        public static string _dateTime = String.Format("{0:yyyyMMdd}_", DateTime.UtcNow);
        public static string _counter = "counter";
        public static string path = @$"E:\IT\Repositories\SushiConsoleDev\SushiConsoleDev\SushiConsole\Logger\LoggerRepository\log{_dateTime}[{_counter}].txt";
        public static  int ThreadInfo { get; private set; } = Thread.CurrentThread.ManagedThreadId;

        public static void Info(string layer, Type type, string method, string message, string threadInfo)
        {
            LoggerHelper(layer, type, method, message, threadInfo);

        }

        public static void Debug(string layer, Type type, string method, string message, string threadInfo)
        {
            LoggerHelper(layer, type, method, message, threadInfo);
        }
     
        public static void Error(string layer, Type type, string method, string message, string threadInfo)
        {
            LoggerHelper(layer, type, method, message, threadInfo);
        }

        public void CheckFileSize()
        {

        }
        public static void LoggerHelper(string layer, Type type, string method, string message, string threadInfo)
        {
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                StringBuilder sb = new StringBuilder();
                var time = String.Format("{0:HH:mm:ss dd/MM/yyyy} ", DateTime.Now);
                var fromNamespaceType = type.ToString();
                sb.Append(time);
                sb.Append($"[{layer}] ");
                sb.Append($"{fromNamespaceType}" + $".{method}_");
                sb.Append($"{message}_");
                sb.Append($"thread id:{threadInfo}");
                sb.ToString();
                streamWriter.WriteLine(sb.ToString());

            }
        }
    }
}