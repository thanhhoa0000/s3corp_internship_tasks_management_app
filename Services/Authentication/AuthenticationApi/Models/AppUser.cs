namespace TaskManagementApp.Services.AuthenticationApi.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        [Required, MinLength(2), MaxLength(30)]
        public required string FirstName { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public required string LastName { get; set; }


        public AdminUser? AdminUser { get; set; }
        public NormalUser? NormalUser { get; set; }
    }
}
