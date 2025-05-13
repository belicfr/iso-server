using System.Text;
using Iso.Data.DbContexts;
using Iso.Data.Managers;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService;
using Iso.Data.Services.DRoomTemplateService;
using Iso.Data.Services.DUserService;
using Iso.WebSocket.Hubs;
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
builder.Services.Configure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string? accessToken = context.Request.Query["access_token"];
                PathString path = context.HttpContext.Request.Path;

                if (!string.IsNullOrWhiteSpace(accessToken)
                    && path.StartsWithSegments("/ws"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<CompleteUserManager>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.AllowedUserNameCharacters
        = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
    options.User.RequireUniqueEmail = true;
});


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<RoomTemplateService>();

builder.Services.AddSingleton<RoomRuntimeService>();
builder.Services.AddSingleton<UserRuntimeService>();


// If client is on another domain
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", policyBuilder => 
        policyBuilder
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["JwtIssuer"],
            ValidAudience            = builder.Configuration["JwtIssuer"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JWTSigningKey"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("jwt", out var jwt))
                {
                    context.Token = jwt;
                }
                
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSignalR();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();  // TODO: on prod

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// app.MapIdentityApi<User>();

app.MapHub<GameHub>("/ws/game")
    .RequireAuthorization();

app.Run();