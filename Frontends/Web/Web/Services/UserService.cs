
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
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/roles"
            }, bearer: true);
        }

        public async Task<Response?> CreateUserAsync(CreateUserRequest request)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Post,
                Body = request,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/users"
            }, bearer: true);
        }

        public async Task<Response?> DeleteRoleAsync(Guid roleId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Delete,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/roles/{roleId}"
            }, bearer: true);
        }

        public async Task<Response?> DeleteUserAsync(Guid userId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Delete,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/users/{userId}"
            }, bearer: true);
        }

        public async Task<Response?> GetRoleAsync(Guid roleId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/roles/{roleId}"
            }, bearer: true);
        }

        public async Task<Response?> GetRolesAsync()
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/roles"
            }, bearer: true);
        }

        public async Task<Response?> GetUserAsync(Guid userId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/users/{userId}"
            }, bearer: true);
        }

        public async Task<Response?> GetUsersAsync()
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/users"
            }, bearer: true);
        }

        public async Task<Response?> UpdateRoleAsync(AppRoleDto roleDto)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Put,
                Body = roleDto,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/roles"
            }, bearer: true);
        }

        public async Task<Response?> UpdateUserAsync(AppUserDto userDto)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Put,
                Body = userDto,
                Url = $"{ApiUrlProperties.ApiGatewayUrl}/users"
            }, bearer: true);
        }
    }
}
