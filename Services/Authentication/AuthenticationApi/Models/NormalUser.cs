namespace TaskManagementApp.Services.AuthenticationApi.Models
{
    public class NormalUser : IdentityUser<Guid>, IEntity
    {
        public required AppUser AppUser { get; set; }
    }
}
