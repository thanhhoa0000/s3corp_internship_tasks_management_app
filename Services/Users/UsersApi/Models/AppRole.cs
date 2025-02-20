namespace TaskManagementApp.Services.UsersApi.Models
{
    public class AppRole : IdentityRole<Guid>, IEntity
    {
        [StringLength(30)]
        public string? Description { get; set; }
    }
}
