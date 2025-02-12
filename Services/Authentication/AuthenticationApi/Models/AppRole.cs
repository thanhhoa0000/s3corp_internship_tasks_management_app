namespace TaskManagementApp.Services.AuthenticationApi.Models
{
    public class AppRole : IdentityRole<Guid>
    {
        [StringLength(30)]
        public string? Description { get; set; }
    }
}
