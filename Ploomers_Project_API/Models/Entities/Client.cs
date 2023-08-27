using Ploomers_Project_API.Models.Enums;

namespace Ploomers_Project_API.Models.Entities
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public DocTypeEnum Type { get; set; }
        public string Document { get; set; }
        public string Address { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Sale> Sales { get; set; }
    }
}
