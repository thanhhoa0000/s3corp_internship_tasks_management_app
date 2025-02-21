namespace TaskManagementApp.Services.UsersApi.Repositories
{
    public class UserRepository : Repository<AppUser, UserContext>, IUserRepository
    {
        

        public UserRepository(IDbContextFactory<UserContext> contextFactory) : base(contextFactory)
        {
            
        }

        public async Task AssignAdminRoleAsync(Guid userId)
        {
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

            var userRole = new IdentityUserRole<Guid> { UserId = userId, RoleId = adminRole!.Id };
            await _context.Set<IdentityUserRole<Guid>>().AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
