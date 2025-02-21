namespace TaskManagementApp.Services.UsersApi.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task AssignAdminRoleAsync(Guid userId);
    }
}
