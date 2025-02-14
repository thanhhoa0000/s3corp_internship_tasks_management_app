using System.ComponentModel;

namespace TaskManagementApp.Services.UsersApi.Models.Dtos
{
    public class CreateUserRequest
    {
        public required AppUserDto User { get; set; }
        [PasswordPropertyText]
        public required string DefaultPassword { get; set; } = "Aa@123456";
        public required AppRole Role { get; set; }
    }
}
