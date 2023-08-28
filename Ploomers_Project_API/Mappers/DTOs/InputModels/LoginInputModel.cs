namespace Ploomers_Project_API.Mappers.DTOs.InputModels
{
    // How the login data is expected to arrive from requests
    public class LoginInputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
