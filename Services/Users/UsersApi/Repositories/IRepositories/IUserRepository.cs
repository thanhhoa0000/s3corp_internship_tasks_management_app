namespace TaskManagementApp.Services.UsersApi.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task AssignRoleAsync(Guid userId, Guid roleId);
    }
}
