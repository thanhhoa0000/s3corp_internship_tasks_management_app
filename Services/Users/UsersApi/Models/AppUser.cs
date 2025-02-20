namespace TaskManagementApp.Services.UsersApi.Models
{
    public class AppUser : IdentityUser<Guid>, IEntity
    {
        [MinLength(2), MaxLength(30)]
        public string? FirstName { get; set; }
        [MinLength(2), MaxLength(50)]
        public string? LastName { get; set; }


        public AdminUser? AdminUser { get; set; }
        public NormalUser? NormalUser { get; set; }
    }
}
