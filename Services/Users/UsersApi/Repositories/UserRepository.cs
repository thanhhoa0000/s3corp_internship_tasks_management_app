namespace TaskManagementApp.Services.UsersApi.Repositories
{
    public class UserRepository : Repository<AppUser, UserContext>, IUserRepository
    {
        

        public UserRepository(IDbContextFactory<UserContext> contextFactory) : base(contextFactory)
        {
            
        }

        public async Task AssignRoleAsync(Guid userId, Guid roleId)
        {
            var userRole = new IdentityUserRole<Guid> { UserId = userId, RoleId = roleId };
            await _context.Set<IdentityUserRole<Guid>>().AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
