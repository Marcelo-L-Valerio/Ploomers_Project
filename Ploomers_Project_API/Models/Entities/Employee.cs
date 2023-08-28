namespace Ploomers_Project_API.Models.Entities
{
    public class Employee : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpiryTime { get; set; }
        public required List<Sale> Sales { get; set; }
    }
}
