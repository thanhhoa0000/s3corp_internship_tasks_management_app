namespace TaskManagementApp.Services.AuthenticationApi.Services.IServices
{
    public interface ITokenProvider
    {
        string CreateAccessToken(AppUser user, IEnumerable<Role> roles);
        string GenerateRefreshToken();
    }
}
