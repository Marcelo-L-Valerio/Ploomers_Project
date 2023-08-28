namespace Ploomers_Project_API.Models.Entities
{
    public class Sale : BaseEntity
    {
        public required string Product { get; set; }
        public required int Amount { get; set; }
        public required int Value { get; set; }
        public required DateTime Date { get; set; }
        public required Guid ClientId { get; set; }
        public required Guid EmployeeId { get; set; }
        public required virtual Client Client { get; set; }
        public required virtual Employee Employee { get; set; }
    }
}
