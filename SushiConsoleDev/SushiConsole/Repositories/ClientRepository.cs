using SushiConsole.Exceptions;
using SushiConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private List<Client> _clients = new List<Client>();

        public void CreateClient(Client client)
        {
            _clients.Add(client);
        }
       
        public List<Client> GetAllClients()
        {
            var allClients = _clients.ToList();

            foreach (var client in allClients)
            {
                Console.WriteLine($"Id - {client.Id}, Name - {client.Name}, email - {client.Email}, address - {client.Address}");
            }
            return allClients;
        }

        public Client GetClientById(Guid clientId)
        {
            var clientToGet = _clients.FirstOrDefault(c => c.Id == clientId);
            return clientToGet;
        }

        public void ShowAllClients()
        { 
            foreach (Client client in _clients)
            { 
                //Console.WriteLine($"Id - {client.Id}, Name - {client.Name}, email - {client.Email}, address - {client.Address}");
            }
        }

        #region GetClientByName
        public Client GetClientByName(string name)
        {
            var clientToGet = _clients.FirstOrDefault(c => c.Name == name);
            return clientToGet;
        }
        #endregion

        #region UpdateClient
        public void UpdateClient(Client client)
        {
            var clientToUpdate = _clients.FirstOrDefault(c => c.Id == client.Id);
            if (clientToUpdate != null)
            {
                clientToUpdate.Name = client.Name;
                clientToUpdate.Email = client.Email;
                clientToUpdate.Address = client.Address;
            }
        }
        #endregion

        #region DeleteClient
        public void DeleteClient(Client client)
        {
            var clientToDelete = _clients.FirstOrDefault(c => c.Id == client.Id);
            if (clientToDelete != null)
            {
                _clients.Remove(clientToDelete);
            }
            else
            {
                throw new ClientNotFoundException("Client is not found");
            }
        }
        #endregion
    }
}