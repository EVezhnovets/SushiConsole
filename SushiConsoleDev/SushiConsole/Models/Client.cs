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
        private string _email;
        private string _adress;

        public Guid Id
        {
            get
            {
                Logger.Info(Logger.info, type, "Id", "Call get method", $"{Logger.ThreadInfo}");

                return _id;
            }
             private set
            {
                Logger.Info(Logger.info, type, "Id", "Call get method", $"{Logger.ThreadInfo}");
            }
        }
        public string Name 
        {
            get
            {
                Logger.Info(Logger.info, type, "Name", "Call get method", $"{Logger.ThreadInfo}");
                
                return _name;
            }
            set
            {
                _name = value;

                Logger.Info(Logger.info, type, "Name", "Call set method", $"{Logger.ThreadInfo}");
            } 
        }
        public string Email {
            get 
            {
                Logger.Info(Logger.info, type, "Email", "Call get method", $"{Logger.ThreadInfo}");

                return _email; 
            } 
            set
            {
                _email = value;
                Logger.Info(Logger.info, type, "Email", "Call set method", $"{Logger.ThreadInfo}");
            }
        }
        public string Address 
        { 
            get
            {
                Logger.Info(Logger.info, type, "Address", "Call get method", $"{Logger.ThreadInfo}");

                return _adress;
            } 
            set
            {
                Logger.Info(Logger.info, type, "Address", "Call set method", $"{Logger.ThreadInfo}");

                _adress = value;
            } 
        }

        public Client()
        {
            Id = _id;

            Logger.Info(Logger.info, type, "Client", "Call set method", $"{Logger.ThreadInfo}");
        }
        public Client(string name, string email)
        {
            Id = _id;
            Name = name;
            Email = email;

            Logger.Info(Logger.info, type, "Client", "Call set method", $"{Logger.ThreadInfo}");
        }
    }
}