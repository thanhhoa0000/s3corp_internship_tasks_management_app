namespace TaskManagementApp.Frontends.Web.Models
{
    public class LoginResponse
    {
        public required AppUserDto User { get; set; }
        public required string Token { get; set; }
    }
}
