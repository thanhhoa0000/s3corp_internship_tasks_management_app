namespace TaskManagementApp.Services.UsersApi.Models.Dtos
{
    public class CreateUserRequest
    {
        public required AppUserDto User { get; set; }
        public required AppRole Role { get; set; }
    }
}
