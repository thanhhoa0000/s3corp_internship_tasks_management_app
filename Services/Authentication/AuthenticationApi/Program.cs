var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Use NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Configuration.AddJsonFile("jwt_properties.json", optional: false, reloadOnChange: true);

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddDbContextFactory<AuthenticationContext>(options
        => options.UseSqlServer(
            builder.Configuration.GetConnectionString("UsersDB"),
            sqlOptions => sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(15),
                errorNumbersToAdd: null)
            ));

    builder.Services.AddIdentity<AppUser, AppRole>()
        .AddEntityFrameworkStores<AuthenticationContext>()
        .AddDefaultTokenProviders();

    builder.Services.Configure<JwtProperties>(builder.Configuration.GetSection("JwtProperties"));

    builder.Services.AddSingleton<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<ITokenProvider, TokenProvider>();


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();

    builder.Services.AddCarter();

    builder.Services.AddAutoMapper(config =>
    {
        config.CreateMap<AppUser, AppUserDto>();
        config.CreateMap<NormalUser, NormalUserDto>();
        config.CreateMap<AdminUser, AdminUserDto>();
        config.CreateMap<RegistrationRequest, AppUser>();
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.MapCarter();

    app.UseHttpsRedirection();

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
