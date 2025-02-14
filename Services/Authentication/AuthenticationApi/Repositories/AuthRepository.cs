using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TaskManagementApp.Services.AuthenticationApi.Repositories
{
    public class AuthRepository : IAuthRepository, IDisposable
    {
        private readonly AuthenticationContext _context;
        internal DbSet<AppUser> _dbSet;
        private bool _disposed = false;

        public AuthRepository(IDbContextFactory<AuthenticationContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
            _dbSet = _context.AppUsers;
        }

        public async Task<AppUser>
            GetUserAsync(Expression<Func<AppUser, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<AppUser> query = _dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            AppUser? user = await query.FirstOrDefaultAsync();

            return user!;
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
