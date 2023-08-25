using Ploomers_Project_API.DTOs.ViewModels;
using Ploomers_Project_API.Mappers.DTOs.InputModels;

namespace Ploomers_Project_API.Business
{
    public interface IClientBusiness
    {
        ClientViewModel Create(ClientInputModel person);
        ClientViewModel FindById(Guid id);
        void Update(Guid id, ClientInputModel person);
        void Delete(Guid id);
        List<ClientViewModel> FindAll();
        public void AddContact(Guid id, ContactInputModel contact);
        public void DeleteContact(Guid idcli, Guid idcont);
    }
}
