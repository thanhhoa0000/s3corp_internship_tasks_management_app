namespace TaskManagementApp.Services.UsersApi.Models
{
    public class AdminUser : IdentityUser<Guid>
    {
        public required AppUser AppUser { get; set; }
    }
}
