namespace TaskManagementApp.Services.AuthenticationApi.Models
{
    public class NormalUser : IdentityUser<Guid>
    {
        public required AppUser AppUser { get; set; }
    }
}
