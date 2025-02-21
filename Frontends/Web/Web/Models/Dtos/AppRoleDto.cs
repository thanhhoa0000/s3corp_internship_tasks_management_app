namespace TaskManagementApp.Frontends.Web.Models.Dtos
{
    public class AppRoleDto : IdentityRole<Guid>
    {
        [StringLength(30)]
        public string? Description { get; set; }
    }
}
