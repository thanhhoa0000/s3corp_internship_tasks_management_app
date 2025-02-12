namespace TaskManagementApp.Services.UsersApi.Models
{
    public class NormalUser : IdentityUser<Guid>
    {
        public required AppUser AppUser { get; set; }
    }
}
