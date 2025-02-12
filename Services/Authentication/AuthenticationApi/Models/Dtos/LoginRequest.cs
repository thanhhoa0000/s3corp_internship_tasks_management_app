using System.ComponentModel;

namespace TaskManagementApp.Services.AuthenticationApi.Models.Dtos
{
    public class LoginRequest
    {
        [Required, MinLength(6)]
        public required string UserName { get; set; }
        [Required, PasswordPropertyText]
        public required string Password { get; set; }
    }
}
