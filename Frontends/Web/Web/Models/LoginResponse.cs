namespace TaskManagementApp.Frontends.Web.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("accessToken")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public required string RefreshTokenToken { get; set; }
    }
}
