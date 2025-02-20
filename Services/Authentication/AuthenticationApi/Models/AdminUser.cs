namespace TaskManagementApp.Services.AuthenticationApi.Models
{
    public class AdminUser : IdentityUser<Guid>, IEntity
    {
        public required AppUser AppUser { get; set; }
    }
}
