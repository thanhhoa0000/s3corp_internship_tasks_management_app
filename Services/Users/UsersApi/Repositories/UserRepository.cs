
using Microsoft.AspNetCore.Identity;

namespace TaskManagementApp.Services.UsersApi.Repositories
{
    public class UserRepository : Repository<AppUser, UserContext>, IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(IDbContextFactory<UserContext> contextFactory) : base(contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }

        public async Task AssignRoleAsync(Guid userId, Guid roleId)
        {
            var userRole = new IdentityUserRole<Guid> { UserId = userId, RoleId = roleId };
            await _context.Set<IdentityUserRole<Guid>>().AddAsync(userRole);
            await SaveAsync();
        }
    }
}
