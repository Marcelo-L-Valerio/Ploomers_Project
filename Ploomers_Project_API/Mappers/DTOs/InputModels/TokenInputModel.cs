namespace Ploomers_Project_API.Mappers.DTOs.InputModels
{
    // How the token data is expected to arrive from requests
    // this class is for refresh token requests
    public class TokenInputModel
    {
        public TokenInputModel(bool authenticated, string accessToken, string refreshToken)
        {
            Authenticated = authenticated;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public bool Authenticated { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
