namespace Ploomers_Project_API.Mappers.DTOs.InputModels
{
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
