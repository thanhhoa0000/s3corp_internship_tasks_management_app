namespace TaskManagementApp.Services.AuthenticationApi.Services.IServices
{
    public interface ITokenProvider
    {
        string CreateToken(AppUser user, IEnumerable<Role> roles);
    }
}
