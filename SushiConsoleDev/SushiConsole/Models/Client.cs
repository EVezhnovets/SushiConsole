using SushiConsoleDev.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Models
{
    public class Client
    {
        Type type = typeof(Client);
       
        private string _name;
        private Guid _id = Guid.NewGuid();

        public Guid Id
        {
            get
            {
                Logger.Info(type, type.GetProperty("Id").ToString(), "Call get method", $"{Thread.CurrentThread.ManagedThreadId}");

                return _id;
            }
             private set
            {
                Logger.Info(type, type.GetProperty("Id").ToString(), "Call get method", $"{Thread.CurrentThread.ManagedThreadId}");
            }
        }
        public string Name 
        {
            get
            {
                Logger.Info(type, type.GetProperty("Name").ToString(), "Call get method", $"{Thread.CurrentThread.ManagedThreadId}");
                
                return _name;
            }
            set
            {
                _name = value;

                Logger.Info(type, type.GetProperty("Name").ToString(), "Call set method", $"{Thread.CurrentThread.ManagedThreadId}");
            } 
        }
        public string Email { get; set; }
        public string Address { get; set; }

        public Client()
        {
            Id = _id;
        }
        public Client(string name, string email)
        {
            Id = _id;
            Name = name;
            Email = email;
        }
    }
}