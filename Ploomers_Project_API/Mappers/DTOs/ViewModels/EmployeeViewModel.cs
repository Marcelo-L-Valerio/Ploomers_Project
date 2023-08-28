using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Mappers.DTOs.ViewModels
{
    // How the employee data is expected to go back to users
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int LastWeekTotal { get; set; }
        public List<SaleViewModel> LastWeekSales { get; set; }
    }
}
