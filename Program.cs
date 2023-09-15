using SardCoreAPI.Utility.Progress;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI;
using Microsoft.Extensions.Configuration;
using SardCoreAPI.Models.Security.JWT;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SardCoreAPI.Utility.JwtHandler;
using SardCoreAPI.Models.Security.Users;
using Microsoft.AspNetCore.Identity;
using SardCoreAPI.Data;
using SardCoreAPI.Areas.Identity.Data;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SardCoreAPIContextConnection") ?? throw new InvalidOperationException("Connection string 'SardCoreAPIContextConnection' not found.");
Connection.SetGlobalConnectionString(connectionString);
var baseConnectionString = builder.Configuration.GetConnectionString("SardCoreAPIWorldContextConnection") ?? throw new InvalidOperationException("Connection string 'SardCoreAPIWorldContextConnection' not found.");
Connection.SetConnectionString(baseConnectionString);

builder.Services.AddDbContext<SardCoreAPIContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddDefaultIdentity<SardCoreAPIUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SardCoreAPIContext>();

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

builder.Services.AddControllers();
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

/* builder.Services.AddHttpContextAccessor(); */

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
    logger.LogInformation($"Received Request: {request.Method} {request.Path}");

    await next.Invoke();
});

app.MapGet("/", () => "Hello ForwardedHeadersOptions!");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ProgressManager>("/Progress");
});

app.MapControllers();

app.Run();
