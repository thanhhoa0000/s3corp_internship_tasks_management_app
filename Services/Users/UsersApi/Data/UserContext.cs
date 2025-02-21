namespace TaskManagementApp.Services.UsersApi.Data
{
    public class UserContext(DbContextOptions<UserContext> options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<NormalUser> NormalUsers { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().ToTable("AppUsers");

            builder.Entity<AppRole>().ToTable("AppRoles");

            builder.Entity<AdminUser>()
                .HasOne(a => a.AppUser)
                .WithOne(u => u.AdminUser)
                .HasForeignKey<AdminUser>(a => a.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NormalUser>()
                .HasOne(n => n.AppUser)
                .WithOne(u => u.NormalUser)
                .HasForeignKey<NormalUser>(n => n.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppRole>()
                .HasIndex(r => r.Name)
                .IsUnique();

            builder.Entity<AppUser>()
                .HasIndex(r => r.UserName)
                .IsUnique();

            builder.Entity<IdentityUserRole<Guid>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.Entity<IdentityUserRole<Guid>>()
                .ToTable("AspNetUserRoles");
        }
    }
}
