using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Drawing;
using System.Text;

using App.Services;
using Entities;
using App.Api;


// база для api
var builder = WebApplication.CreateBuilder(args);

// конфигурация бд(смотри appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// базовые сервисы
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<AuthService>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// аутентификация идет первая, только потом авторизация
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            }
        };
    });

// база для приложения
var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ¯\_(ツ)_/¯ - не трогать
app.Run(async (context) => await context.Response.SendFileAsync("pupu.jpg"));

// база для api
app.UseHttpsRedirection();
app.MapControllers();

// регистрации api
app.MapGroup("/auth").MapAuthApi();
app.MapGroup("/channels").MapChannelApi();
app.MapGroup("/groups").MapGroupApi();
app.MapGroup("/group_members").MapGroupMemberApi();
app.MapGroup("/messages").MapMessageApi();
app.MapGroup("/notifications").MapNotificationApi();
app.MapGroup("/problems").MapTaskApi();
// app.MapGroup("/task_comments").MapTaskCommentApi();
app.MapGroup("/users").MapUserApi();
app.MapGroup("/user_passwords").MapUserPasswordApi();
app.MapGroup("/user_profiles").MapUserProfileApi();

app.Run();