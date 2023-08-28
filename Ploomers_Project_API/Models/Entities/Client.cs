using Ploomers_Project_API.Models.Enums;

namespace Ploomers_Project_API.Models.Entities
{
    public class Client : BaseEntity
    {
        public required string Name { get; set; }
        // Client type - a company or person
        public required DocTypeEnum Type { get; set; }
        public required string Document { get; set; }
        public required string Address { get; set; }
        public required List<Contact> Contacts { get; set; }
        public required List<Sale> Sales { get; set; }
    }
}
