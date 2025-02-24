var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Use NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/keys/"))
    .SetApplicationName("MyTasks");

    builder.Services.AddControllersWithViews();
    builder.Services.AddHttpContextAccessor();

    builder.Services
        .AddHttpClient("MyTasksApp")
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        });

    builder.Services.AddHttpClient<ITokenProcessor, TokenProcessor>();
    builder.Services.AddHttpClient<IAccountService, AccountService>();
    builder.Services.AddHttpClient<ITaskService, TaskService>();
    builder.Services.AddHttpClient<IUserService, UserService>();
    
    ApiUrlProperties.ApiGatewayUrl = builder.Configuration["GatewayUrl"];

    builder.Services.AddScoped<IBaseService, BaseService>();
    builder.Services.AddScoped<ITokenProcessor, TokenProcessor>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<ITaskService, TaskService>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseAntiforgery();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    logger.Error($"Error(s) occured when starting the app {typeof(Program)}:\n----{ex}");
}
finally
{
    LogManager.Shutdown();
}
