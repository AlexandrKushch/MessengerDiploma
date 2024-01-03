using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Controllers;
using Server.Data;
using Server.Data.Models;
using Server.Data.Models.Configuration;
using Server.Data.Models.Repository;
using Server.Services.Implementations;
using Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddCors();

builder.Services.AddDbContext<MessengerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString"));
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IGPTService, GPTService>();

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MessengerContext>();

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = JwtConfiguration.ISSUER,
        ValidateAudience = true,
        ValidAudience = JwtConfiguration.AUDIENCE,
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = JwtConfiguration.SIGNING_KEY,
        ValidateLifetime = true
    };

    options.Events = new JwtBearerEvents
      {
          OnMessageReceived = context =>
          {
              var accessToken = context.Request.Query["access_token"];

              var path = context.HttpContext.Request.Path;
              if (!string.IsNullOrEmpty(accessToken) &&
                  (path.StartsWithSegments("/messages")))
              {
                  context.Token = accessToken;
              }
              return Task.CompletedTask;
          }
      };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<MessengerContext>();
db.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.MapHub<ChatHub>("/messages");

app.Run();
