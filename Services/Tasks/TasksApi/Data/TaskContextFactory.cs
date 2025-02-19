namespace TaskManagementApp.Services.TasksApi.Data
{
    public class TaskContextFactory : IDesignTimeDbContextFactory<TaskContext>
    {
        public TaskContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TaskContext>();

            var connectionString = configuration.GetConnectionString("TasksDB");

            optionsBuilder.UseSqlServer(connectionString);

            return new TaskContext(optionsBuilder.Options);
        }
    }
}
