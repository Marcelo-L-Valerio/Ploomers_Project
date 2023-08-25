namespace Ploomers_Project_API.Models.Entities
{
    public class Contact : BaseEntity
    {
        public string Type { get; set; }
        public string Info { get; set; }
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}