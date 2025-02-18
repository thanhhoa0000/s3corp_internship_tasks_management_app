using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

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
            var password = builder.Configuration.GetSection("CertPassword").Value;

            var trustedCerts = new List<X509Certificate2>
            {
                new X509Certificate2("/home/app/.aspnet/https/Authen.API.pfx", password),
                new X509Certificate2("/home/app/.aspnet/https/Tasks.API.pfx", password),
                new X509Certificate2("/home/app/.aspnet/https/Users.API.pfx", password)
            };

            var certificatesValidator = new CertificatesValidator(trustedCerts, logger);

            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = certificatesValidator.Validate!
            };
        });

    builder.Services.AddHttpClient<ITokenHandler, TokenHandler>();
    builder.Services.AddHttpClient<IAccountService, AccountService>();

    ApiUrlProperties.TasksUrl = builder.Configuration["ApiUrls:Tasks"];
    ApiUrlProperties.UsersUrl = builder.Configuration["ApiUrls:Users"];
    ApiUrlProperties.AuthUrl = builder.Configuration["ApiUrls:Authentication"];

    builder.Services.AddScoped<IBaseService, BaseService>();
    builder.Services.AddScoped<ITokenHandler, TokenHandler>();
    builder.Services.AddScoped<IAccountService, AccountService>();

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
