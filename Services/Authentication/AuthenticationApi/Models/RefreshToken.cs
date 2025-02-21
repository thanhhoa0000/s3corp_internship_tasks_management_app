namespace TaskManagementApp.Services.AuthenticationApi.Models;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid AppUserId { get; set; }
    [Required]
    public required string Token { get; set; }
    public DateTime ExpireOnUtc { get; set; }
    public AppUser? AppUser { get; set; }
}