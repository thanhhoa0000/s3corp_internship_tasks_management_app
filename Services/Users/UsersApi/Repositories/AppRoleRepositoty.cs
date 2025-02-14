namespace TaskManagementApp.Services.UsersApi.Repositories
{
    public class AppRoleRepositoty : Repository<AppRole, UserContext>, IAppRoleRepository
    {

        public AppRoleRepositoty(IDbContextFactory<UserContext> contextFactory) : base(contextFactory)
        {
            
        }
    }
}
