namespace TaskManagementApp.Services.AuthenticationApi.Models
{
    public class AdminUser : IdentityUser<Guid>
    {
        public required AppUser AppUser { get; set; }
    }
}
