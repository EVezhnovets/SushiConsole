using SushiConsoleDev.Exceptions;
using SushiConsoleDev.Models;
using SushiConsoleDev.Logger;

namespace SushiConsoleDev.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private List<Client> _clients = new List<Client>();

        public void CreateClient(Client client)
        {
            _clients.Add(client);
            Logger.Logger.Info(typeof(ClientRepository), nameof(CreateClient), "Call method");

        }

        public List<Client> GetAllClients()
        {
            var allClients = _clients.ToList();

            Logger.Logger.Info(typeof(ClientRepository), nameof(GetAllClients), "Call method");

            return allClients;
        }

        public Client? GetClientById(Guid clientId)
        {
            var clientToGet = _clients.FirstOrDefault(c => c.Id == clientId);

            Logger.Logger.Info(typeof(ClientRepository), nameof(GetClientById), "Call method");

            return clientToGet;
        }

        public Client? GetClientByName(string name)
        {
            var clientToGet = _clients.FirstOrDefault(c => c.Name == name);

            Logger.Logger.Info(typeof(ClientRepository), nameof(GetClientByName), "Call method");

            return clientToGet;
        }

        public void UpdateClient(Client client)
        {
            var clientToUpdate = _clients.FirstOrDefault(c => c.Id == client.Id);
            if (clientToUpdate != null)
            {
                clientToUpdate.Name = client.Name;
                clientToUpdate.Email = client.Email;
                clientToUpdate.Address = client.Address;
            }
            Logger.Logger.Info(typeof(ClientRepository), nameof(UpdateClient), "Call method");

        }

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
            Logger.Logger.Info(typeof(ClientRepository), nameof(DeleteClient), "Call method");
        }

        public void ClearList()
        {
            _clients.Clear();
        }
    }
}