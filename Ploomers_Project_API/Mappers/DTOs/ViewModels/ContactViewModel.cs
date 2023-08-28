namespace Ploomers_Project_API.Mappers.DTOs.ViewModels
{
    // How the contact data is expected to go back to users
    public class ContactViewModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Info { get; set; }
    }
}
