using SardCoreAPI.Utility.Progress;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using SardCoreAPI.Utility.JwtHandler;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Services.Security;
using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Services.Easy;
using SardCoreAPI.Services.MenuItems;
using SardCoreAPI.Services.WorldContext;
using SardCoreAPI.Services.Setting;
using SardCoreAPI.Services.DataPoints;
using SardCoreAPI.DataAccess;
using SardCoreAPI.Services.Pages;
using SardCoreAPI.Database.DBContext;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Database;
using SardCoreAPI.Services.Hub;
using SardCoreAPI.Models.Security;
using Newtonsoft.Json;
using SardCoreAPI.Services.Content;
using SardCoreAPI.Services.Maps;
using SardCoreAPI.Services.Tasks;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SardCoreAPIContextConnection") ?? throw new InvalidOperationException("Connection string 'SardCoreAPIContextConnection' not found.");
Connection.SetGlobalConnectionString(connectionString);
var libraryConnectionString = builder.Configuration.GetConnectionString("SardCoreAPIWorldContextConnection") ?? throw new InvalidOperationException("Connection string 'SardCoreAPIWorldContextConnection' not found.");
Connection.SetConnectionString(libraryConnectionString);

builder.Services.AddDbContext<SardCoreDBContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddDbContext<SardLibraryDBContext>(options =>
    options.UseMySql(libraryConnectionString, new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddDefaultIdentity<SardCoreAPIUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SardCoreDBContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

Console.WriteLine(builder.Host);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            builder => builder.WithOrigins("http://localhost:4200"));
        options.AddPolicy("AllowOrigin",
            builder => builder.WithOrigins("http://localhost:8080"));
        options.AddPolicy("AllowOrigin",
            builder => builder.WithOrigins("https://libratlas.net"));
    });
builder.Services.AddCors(options => { options.AddDefaultPolicy(policy => { policy.WithOrigins("https://imgur.com/", "*"); }); } );

// JWT Services
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
string key = jwtSettings.GetSection("SecretKey").Value;
builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    }
);
builder.Services.AddScoped<JwtHandler>();

builder.Services.AddScoped<IDataPointTypeDataAccess, DataPointTypeDataAccess>();
builder.Services.AddScoped<IEasyDataAccess, EasyDataAccess>();

builder.Services.AddSingleton<TaskService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<TaskService>());

builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<ISettingJSONService, SettingJSONService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IWorldInfoService, WorldInfoService>();
builder.Services.AddScoped<IEasyQueryService, MySQLEasyQueryService>();
builder.Services.AddScoped<IDataPointService, DataPointService>();
builder.Services.AddScoped<IDataPointQueryService, MySQLDataPointQueryService>();
builder.Services.AddScoped<IViewService, ViewService>();
builder.Services.AddScoped<IDataPointTypeService, DataPointTypeService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IMapService, MapService>();
builder.Services.AddScoped<IMapTileService, MapTileService>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<IDatabaseService, EFCoreDatabaseService>();
builder.Services.AddScoped<IWorldService, WorldService>();
builder.Services.AddScoped<IGenericDataAccess, GenericDataAccess>();

builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddHttpContextAccessor();


builder.Services.Configure<HostOptions>(x =>
{
    x.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
    x.ServicesStartConcurrently = true;
    x.ServicesStopConcurrently = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
    builder.WithOrigins("http://localhost:4200")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials());

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Use(async (context, next) =>
{
    var request = context.Request;
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogTrace($"Received Request: {request.Method} {request.Path}");

    await next.Invoke();
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ProgressManager>("/Progress");
});
app.MapControllers();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<IDatabaseService>().UpdateDatabase();
await app.Services.CreateScope().ServiceProvider.GetRequiredService<IDatabaseService>().UpdateWorldDatabases();
await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISecurityService>().InitializeDefaultUsers();
await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISecurityService>().InitializeWorldsWithDefaultRoles();

app.Run();
