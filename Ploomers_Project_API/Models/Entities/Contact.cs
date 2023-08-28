namespace Ploomers_Project_API.Models.Entities
{
    public class Contact : BaseEntity
    {
        // Contact type - email, cellphone, etc
        public required string Type { get; set; }
        public required string Info { get; set; }
        public required Guid ClientId { get; set; }
        public required virtual Client Client { get; set; }
    }
}