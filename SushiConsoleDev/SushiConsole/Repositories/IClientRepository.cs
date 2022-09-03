using SushiConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Repositories
{
    public interface IClientRepository
    {
        Client GetClientById(Guid clientId);
        Client GetClientByName(string name);
        List<Client> GetAllClients();
        void CreateClient(Client client);
    }
}