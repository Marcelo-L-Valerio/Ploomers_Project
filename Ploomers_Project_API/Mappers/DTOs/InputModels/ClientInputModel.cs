using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Mappers.DTOs.InputModels
{
    public class ClientInputModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Document { get; set; }
        public string Address { get; set; }
    }
}
