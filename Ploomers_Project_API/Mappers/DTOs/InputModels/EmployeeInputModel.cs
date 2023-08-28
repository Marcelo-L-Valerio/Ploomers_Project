namespace Ploomers_Project_API.Mappers.DTOs.InputModels
{
    // How the employee data is expected to arrive from requests
    public class EmployeeInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
