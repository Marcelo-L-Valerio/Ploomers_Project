using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Repository
{
    public interface IClientRepository
    {
        Client Create(Client client);
        Client FindById(Guid id);
        Client Update(Client client);
        void Delete(Guid id);
        List<Client> FindAll();
        public void AddContact(Guid id, Contact contact);
        public void DeleteContact(Guid idcli, Guid idcont);
    }
}
