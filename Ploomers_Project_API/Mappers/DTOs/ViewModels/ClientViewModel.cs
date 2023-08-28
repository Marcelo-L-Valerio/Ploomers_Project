using Ploomers_Project_API.Mappers.DTOs.ViewModels;

namespace Ploomers_Project_API.DTOs.ViewModels
{
    // How the client data is expected to go back to users
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Document { get; set; }
        public string Address { get; set; }
        public int LastWeekTotal { get; set; }
        public List<ContactViewModel> Contacts { get; set; }
        public List<SaleViewModel> LastWeekSales { get; set; }
    }
}
