string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = configurationBuilder.Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .MinimumLevel.Verbose()
    .Enrich.WithProperty("ApplicationContext", "Identity.API")
    .Enrich.WithProcessName()
    .Enrich.WithProcessId()
    .Enrich.WithFunction("Severity", e => $"{e.Level}")
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/Identity.API-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("Configuring web host (Identity.API)...");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your valid token in the text input below.\n\nExample: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddOptions();

builder.Services.AddCors(options =>
{
    options
        .AddPolicy("CorsPolicy", builder => builder.SetIsOriginAllowed((host) => true)
        .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var filters = new List<Type>()
{
    typeof(HttpGlobalExceptionFilter)
};

builder.Services.AddControllers(options =>
{
    foreach (var filter in filters)
    {
        options.Filters.Add(filter);
    }
}).AddJsonOptions(options => 
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseSqlite("Data Source=identity.db", opt =>
    {
        opt.MigrationsAssembly(typeof(IdentityContext).Assembly.GetName().Name);
    });

    options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
});

builder.Services.AddScoped<DbContext, IdentityContext>();
builder.Services.AddScoped<IDbSeeder<IdentityContext>, IdentityContextSeed>();

builder.Services.AddHttpContextAccessor();

#region Repository and Service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITokenService, TokenService>();
#endregion

#region MediatR
builder.Services.AddMediatR(new MediatRServiceConfiguration().RegisterServicesFromAssemblies(typeof(RegisterUserCommandHandler).Assembly));
#endregion

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"], 
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapControllers();

app.UseCors("CorsPolicy"); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        Task.Run(async () =>
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            var seed = scope.ServiceProvider.GetRequiredService<IDbSeeder<IdentityContext>>();

            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(IdentityContext).Name);

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await dbContext.Database.MigrateAsync();
                await seed.SeedAsync(dbContext);
            });
        }).Wait();

    }
    catch (Exception ex)
    {
        logger.LogError("An error occured while running auto-migration");
        logger.LogError(ex.ToString());
    }
}

Log.Information("Starting web host (Identity.API)...");

app.Run();