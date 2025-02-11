namespace TaskManagementApp.SharedLibraries.BaseSharedLibraries.Repositories
{
    public class Repository<T, TContext> : IRepository<T>, IDisposable 
        where T : class
        where TContext : DbContext
    {
        private readonly TContext _context;
        internal DbSet<T> _dbSet;
        private bool _disposed = false;

        public Repository(IDbContextFactory<TContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
            _dbSet = _context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T>
            GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            T? T = await query.FirstOrDefaultAsync();

            return T!;
        }

        public async Task<IEnumerable<T>>
            GetAllAsync(
                Expression<Func<T, bool>>? filter = null,
                bool tracked = true,
                int pageSize = 0,
                int pageNumber = 1)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            if (pageSize > 0)
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            IEnumerable<T> usersList = await query.ToListAsync();

            return usersList;
        }

        public async Task UpdateAsync(T user)
        {
            _dbSet.Update(user);
            await SaveAsync();
        }

        public async Task RemoveAsync(T user)
        {
            _dbSet.Remove(user);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
