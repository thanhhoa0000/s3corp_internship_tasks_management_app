namespace TaskManagementApp.Frontends.Web.Models.Dtos
{
    public class AppUserDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }
        [EmailAddress]
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [Phone]
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
        [JsonPropertyName("firstName")]
        [MinLength(2), MaxLength(30)]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        [MinLength(2), MaxLength(50)]
        public string? LastName { get; set; }
    }
}
