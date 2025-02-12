namespace TaskManagementApp.Services.AuthenticationApi.Models.Dtos
{
    public class LoginResponse
    {
        public required AppUserDto User { get; set; }
        public required string Token { get; set; }
    }
}
