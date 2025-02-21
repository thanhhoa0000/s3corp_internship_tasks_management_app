using Microsoft.EntityFrameworkCore.Design;

namespace TaskManagementApp.Services.AuthenticationApi.Data;

public class AuthenticationContextFactory : IDesignTimeDbContextFactory<AuthenticationContext>
{
    public AuthenticationContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuthenticationContext>();

        var connectionString = configuration.GetConnectionString("UsersDB");

        optionsBuilder.UseSqlServer(connectionString);

        return new AuthenticationContext(optionsBuilder.Options);
    }
}