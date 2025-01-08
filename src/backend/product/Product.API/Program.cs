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
    .Enrich.WithProperty("ApplicationContext", "Product.API")
    .Enrich.WithProcessName()
    .Enrich.WithProcessId()
    .Enrich.WithFunction("Severity", e => $"{e.Level}")
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/Product.API-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("Configuring web host (Product.API)...");

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

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ProductContext>(options =>
{
    options.UseSqlite("Data Source=product.db", opt =>
    {
        opt.MigrationsAssembly(typeof(ProductContext).Assembly.GetName().Name);
    });

    options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
});

builder.Services.AddScoped<DbContext, ProductContext>();
builder.Services.AddScoped<IDbSeeder<ProductContext>, ProductContextSeed>();

builder.Services.AddHttpContextAccessor();

#region Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
#endregion

#region MediatR
builder.Services.AddMediatR(new MediatRServiceConfiguration().RegisterServicesFromAssemblies(typeof(CreateProductCommandHandler).Assembly));
#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddAuthentication("TestScheme")
        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
};

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapControllers();

app.UseCors("CorsPolicy"); 

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
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
            var seed = scope.ServiceProvider.GetRequiredService<IDbSeeder<ProductContext>>();

            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(ProductContext).Name);

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

Log.Information("Starting web host (Product.API)...");

app.Run();

public partial class Program { } 