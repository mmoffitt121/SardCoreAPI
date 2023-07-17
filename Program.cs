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
using SardCoreAPI.DataAccess.Security.Users;

var builder = WebApplication.CreateBuilder(args);

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
});
builder.Services.AddScoped<JwtHandler>();

// Identity Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserStore<User>, UserDataAccess>();
builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleDataAccess>();
builder.Services.AddScoped<IUserValidator<User>, SardCoreAPI.DataAccess.Security.Users.UserValidator>();
builder.Services.AddScoped<IPasswordValidator<User>, SardCoreAPI.DataAccess.Security.Users.PasswordValidator>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
builder.Services.AddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
builder.Services.AddScoped<IUserConfirmation<User>, DefaultUserConfirmation<User>>();
builder.Services.AddScoped<IdentityErrorDescriber>();
builder.Services.AddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
builder.Services.AddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<User>>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, IdentityRole>>();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

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

app.Use(async (context, next) =>
{
    var request = context.Request;
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Received Request: {request.Method} {request.Path}");

    await next.Invoke();
});

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
