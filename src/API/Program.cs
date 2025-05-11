using System.Text;
using Iso.Data.DbContexts;
using Iso.Data.Managers;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AuthDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));

builder.Services.AddDbContext<GameDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Configuration.AddKeyPerFile(
    builder.Configuration["ResourcesDirectory"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWTSigningKey"]!))
        };
    });

builder.Services.AddScoped<CompleteUserManager>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.AllowedUserNameCharacters
        = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
    options.User.RequireUniqueEmail = true;
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// app.MapIdentityApi<User>();

app.Run();