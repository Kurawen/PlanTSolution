using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Drawing;
using System.Net.Http;
using App.Services;
using Entities;
using App.Api;
using System;

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

// ValidateIssuer: указывает, будет ли валидироваться издатель при валидации токена
// ValidateAudience: указывает, будет ли валидироваться потребитель токена
// ValidateLifetime: указывает, будет ли валидироваться время существования
// ValidateIssuerSigningKey: указывает, будет ли валидироваться ключ безопасности
// ValidIssuer: строка, которая представляет издателя токена
// ValidAudience: строка, которая представляет потребителя токена
// IssuerSigningKey: представляет ключ безопасности - объект SecurityKey, который будет применяться при генерации токена

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

// все для токенов
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

// генерация токена для пользователя (для логина)
app.MapPost("/login/{username}", async (string username, [FromBody] string password, AppDbContext db) =>
{
    var user = await
        db.Users.FirstOrDefaultAsync(u => u.First_Name == username);

    // if (user is null || user.Password?.Password is null)
    //     return Results.Unauthorized();

    byte[] hash = SHA512.HashData(Encoding.UTF8.GetBytes(password));
    string hex = BitConverter.ToString(hash).Replace("-", "");

    // if (hex != user.Password.Password)
    //     return Results.Unauthorized();

    var claims = new List<Claim>
    {
        new(ClaimTypes.Name, username),
        // new("id", user.Id.ToString())
    };

    // if (user.Roles?.Any() == true)
    //     foreach (var role in user.Roles!.Select(r => r.Role!.Name))
    //         claims.Add(new(ClaimTypes.Role, role));

    var jwt = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            SecurityAlgorithms.HmacSha256));

    return Results.Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
});


// app.Map("/data", [Authorize] (HttpContent context) => $"Bebrou");

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

app.MapGroup("/users").MapUserApi().RequireAuthorization();
// app.MapGroup("/roles").MapRolesApi().RequireAuthorization(a => a.RequireRole("Admin"));

app.Run();