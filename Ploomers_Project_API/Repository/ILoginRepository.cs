using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Repository
{
    public interface ILoginRepository
    {
        Employee ValidateCredentials(Employee employee);
        Employee ValidateCredentials(string email);
        Employee RefreshUserInfo(Employee employee);
        bool RevokeToken(string email);
    }
}
