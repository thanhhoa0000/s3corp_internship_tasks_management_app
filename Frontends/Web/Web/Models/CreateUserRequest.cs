namespace TaskManagementApp.Frontends.Web.Models
{
    public class CreateUserRequest
    {
        public AppUserDto? User { get; set; }
        [PasswordPropertyText]
        public string? DefaultPassword { get; set; } = "Aa@123456";
        public AppRoleDto? Role { get; set; }
    }
}
