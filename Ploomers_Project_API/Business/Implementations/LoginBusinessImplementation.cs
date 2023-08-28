using AutoMapper;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Entities;
using Ploomers_Project_API.Repository;
using Ploomers_Project_API.Services.TokenService;
using Ploomers_Project_API.Services.TokenService.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ploomers_Project_API.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private TokenConfiguration _configuration;
        private ILoginRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public LoginBusinessImplementation(
            TokenConfiguration configuration, ILoginRepository repository,
            ITokenService tokenService, IMapper mapper)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        // Login data validation
        public TokenViewModel ValidateCredentials(LoginInputModel credentials)
        {
            var info = _mapper.Map<Employee>(credentials);
            var employee = _repository.ValidateCredentials(info);

            if (employee == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, employee.Email)
            };

            var accessToken = _tokenService.GenerateAcessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            employee.RefreshToken = refreshToken;
            employee.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

            _repository.RefreshUserInfo(employee);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenViewModel(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        // Refresh token data validation
        public TokenViewModel ValidateCredentials(TokenInputModel token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var email = principal.Identity.Name;

            var employee = _repository.ValidateCredentials(email);

            if((employee == null || 
                employee.RefreshToken != refreshToken || 
                employee.RefreshTokenExpiryTime <= DateTime.Now)) return null;

            accessToken = _tokenService.GenerateAcessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            employee.RefreshToken = refreshToken;

            _repository.RefreshUserInfo(employee);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenViewModel(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        // Revoke token (log off)
        public bool RevokeToken(string email)
        {
            return _repository.RevokeToken(email);
        }
    }
}
