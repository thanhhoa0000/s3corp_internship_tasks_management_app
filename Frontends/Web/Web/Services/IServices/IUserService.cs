namespace TaskManagementApp.Frontends.Web.Services.IServices
{
    public interface IUserService
    {
        Task<Response?> GetUsersAsync();
        Task<Response?> GetUserAsync(Guid userId);
        Task<Response?> CreateUserAsync(CreateUserRequest request);
        Task<Response?> UpdateUserAsync(AppUserDto userDto);
        Task<Response?> DeleteUserAsync(Guid userId);


        Task<Response?> GetRolesAsync();
        Task<Response?> GetRoleAsync(Guid roleId);
        Task<Response?> CreateRoleAsync(string roleName);
        Task<Response?> UpdateRoleAsync(AppRoleDto roleDto);
        Task<Response?> DeleteRoleAsync(Guid roleId);
    }
}
