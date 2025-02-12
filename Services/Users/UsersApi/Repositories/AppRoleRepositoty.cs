namespace TaskManagementApp.Services.UsersApi.Repositories
{
    public class AppRoleRepositoty : Repository<AppRole, UserContext>, IAppRoleRepository
    {
        private readonly UserContext _context;

        public AppRoleRepositoty(IDbContextFactory<UserContext> contextFactory) : base(contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }
    }
}
