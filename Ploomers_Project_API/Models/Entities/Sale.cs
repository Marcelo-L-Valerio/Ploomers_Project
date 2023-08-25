namespace Ploomers_Project_API.Models.Entities
{
    public class Sale : BaseEntity
    {
        public string Product { get; set; }
        public int Amount { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
