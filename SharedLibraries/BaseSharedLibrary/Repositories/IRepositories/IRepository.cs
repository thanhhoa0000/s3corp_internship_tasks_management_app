namespace TaskManagementApp.SharedLibraries.BaseSharedLibraries.Repositories.IRepositories
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>>
            GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, int pageSize = 0, int pageNumber = 1);
        Task<T>
            GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task CreateAsync(T user);
        Task RemoveAsync(T user);
        Task UpdateAsync(T user);
        Task SaveAsync();
    }
}
