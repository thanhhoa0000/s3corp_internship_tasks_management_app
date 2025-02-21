namespace TaskManagementApp.Services.AuthenticationApi.Models.Dtos
{
    public class LoginResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
