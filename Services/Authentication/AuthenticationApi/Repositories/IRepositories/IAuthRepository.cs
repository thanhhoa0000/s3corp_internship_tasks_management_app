using System.Linq.Expressions;

namespace TaskManagementApp.Services.AuthenticationApi.Repositories.IRepositories
{
    public interface IAuthRepository : IDisposable
    {
        Task<AppUser> GetUserAsync(Expression<Func<AppUser, bool>>? filter = null, bool tracked = true);
    }
}
