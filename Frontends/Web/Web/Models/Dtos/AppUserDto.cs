namespace TaskManagementApp.Frontends.Web.Models.Dtos
{
    public class AppUserDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("userName")]
        public required string UserName { get; set; }
        [EmailAddress]
        [JsonPropertyName("email")]
        public required string Email { get; set; }
        [Phone]
        [JsonPropertyName("phoneNumber")]
        public required string PhoneNumber { get; set; }
    }
}
