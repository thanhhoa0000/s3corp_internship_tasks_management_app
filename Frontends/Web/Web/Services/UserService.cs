
namespace TaskManagementApp.Frontends.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseService _service;

        public UserService(IBaseService service)
        {
            _service = service;
        }

        public async Task<Response?> CreateRoleAsync(string roleName)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Post,
                Body = roleName,
                Url = ApiUrlProperties.UsersUrl + "/api/v1/roles"
            }, bearer: true);
        }

        public async Task<Response?> CreateUserAsync(CreateUserRequest request)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Post,
                Body = request,
                Url = ApiUrlProperties.UsersUrl + "/api/v1/users"
            }, bearer: true);
        }

        public async Task<Response?> DeleteRoleAsync(Guid roleId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Delete,
                Url = ApiUrlProperties.UsersUrl + $"/api/v1/roles/{roleId}"
            }, bearer: true);
        }

        public async Task<Response?> DeleteUserAsync(Guid userId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Delete,
                Url = ApiUrlProperties.UsersUrl + $"/api/v1/users/{userId}"
            }, bearer: true);
        }

        public async Task<Response?> GetRoleAsync(Guid roleId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = ApiUrlProperties.UsersUrl + $"/api/v1/roles/{roleId}"
            }, bearer: true);
        }

        public async Task<Response?> GetRolesAsync()
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = ApiUrlProperties.UsersUrl + "/api/v1/roles"
            }, bearer: true);
        }

        public async Task<Response?> GetUserAsync(Guid userId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = ApiUrlProperties.UsersUrl + $"/api/v1/users/{userId}"
            }, bearer: true);
        }

        public async Task<Response?> GetUsersAsync()
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = ApiUrlProperties.UsersUrl + "/api/v1/users"
            }, bearer: true);
        }

        public async Task<Response?> UpdateRoleAsync(AppRoleDto roleDto)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Put,
                Body = roleDto,
                Url = ApiUrlProperties.UsersUrl + "/api/v1/roles"
            }, bearer: true);
        }

        public async Task<Response?> UpdateUserAsync(AppUserDto userDto)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Put,
                Body = userDto,
                Url = ApiUrlProperties.UsersUrl + "/api/v1/users"
            }, bearer: true);
        }
    }
}
