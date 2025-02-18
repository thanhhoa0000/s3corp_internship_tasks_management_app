namespace TaskManagementApp.Frontends.Web.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("user")]
        public required AppUserDto User { get; set; }
        [JsonPropertyName("token")]
        public required string Token { get; set; }
    }
}
