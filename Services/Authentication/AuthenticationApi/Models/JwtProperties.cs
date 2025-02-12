namespace TaskManagementApp.Services.AuthenticationApi.Models
{
    public class JwtProperties
    {
        [JsonPropertyName("iss")]
        public string? Issuer { get; set; }

        [JsonPropertyName("aud")]
        public string? Audience { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("exp")]
        public int? Expiration { get; set; }
    }
}
