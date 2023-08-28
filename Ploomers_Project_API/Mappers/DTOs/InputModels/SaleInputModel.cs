using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Mappers.DTOs.InputModels
{
    // How the sale data is expected to arrive from requests
    public class SaleInputModel
    {
        public string Product { get; set; }
        public int Amount { get; set; }
        public int Value { get; set; }
    }
}
