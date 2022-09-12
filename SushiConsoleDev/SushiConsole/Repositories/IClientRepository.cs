using SushiConsoleDev.Models;

namespace SushiConsoleDev.Repositories
{
    public interface IClientRepository
    {
        Client GetClientById(Guid clientId);
        Client GetClientByName(string name);
        List<Client> GetAllClients();
        void CreateClient(Client client);
    }
}