namespace TaskManagementApp.Services.TasksApi.Data
{
    public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskItem>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}
