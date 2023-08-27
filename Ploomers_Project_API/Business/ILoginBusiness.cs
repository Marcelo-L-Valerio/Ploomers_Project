using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;

namespace Ploomers_Project_API.Business
{
    public interface ILoginBusiness
    {
        TokenViewModel ValidateCredentials(LoginInputModel employee);
        TokenViewModel ValidateCredentials(TokenInputModel token);
        bool RevokeToken(string email);
    }
}
