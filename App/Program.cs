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
// using App.Services;
using Entities;
using App.Api;
using System;
using App.Services;

// база для api
var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    builder.Logging.AddConsole();
});

// конфигурация бд(смотри appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// базовые сервисы
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddScoped<AuthService>();
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// база для приложения
var app = builder.Build();

app.UseCors("AllowViteDev");

// Авто логирование всех Api кроме /login
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("===> Действие {Method} - данные: {Path}",
        context.Request.Method, context.Request.Path);

    var startTime = DateTime.UtcNow;

    await next();

    var elapsed = DateTime.UtcNow - startTime;

    logger.LogInformation("<=== Ошибка {StatusCode} - не удалось получить доступ - время работы: {ElapsedMs} ms",
        context.Response.StatusCode, elapsed.TotalMilliseconds);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// все для токенов
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ¯\_(ツ)_/¯ - не трогать
// app.Run(async (context) => await context.Response.SendFileAsync("pupu.jpg"));   

// база для api
app.UseHttpsRedirection();
app.MapControllers();

// генерация токена для пользователя (для логина)
app.MapPost("/login", async ([FromBody] LoginRequest request, AppDbContext db) =>
{
    app.Logger.LogInformation("Login attempt for: {Email}", request.Email);
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    try
    {
        if (user is null)
            return Results.Unauthorized();

        var userPassword = await db.UserPasswords
            .FirstOrDefaultAsync(up => up.User_id == user.Id);

        if (userPassword is null || userPassword.Hash_password is null)
            return Results.Unauthorized();

        byte[] hash = SHA512.HashData(Encoding.UTF8.GetBytes(request.Password));
        string hex = BitConverter.ToString(hash).Replace("-", "");

        if (hex != userPassword.Hash_password)
            return Results.Unauthorized();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.First_Name ?? ""),
            new(ClaimTypes.Email, user.Email)
        };

        var jwt = new JwtSecurityToken(
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
                SecurityAlgorithms.HmacSha256));
        
        app.Logger.LogInformation("Успешный вход для пользователя: {UserId}", user.Id);

        return Results.Ok(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            UserId = user.Id,
            Email = user.Email
        });
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Ошибка при входе: {Email}", request.Email);
        return Results.Unauthorized();
    }
});

app.MapPost("/register", async ([FromBody] RegisterRequest request, AppDbContext db) =>
{
    var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
    if (existingUser != null)
        return Results.BadRequest("Пользователь с этим Email уже существует!");

    // Создаем пользователя
    var newUser = new User
    {
        Id = Guid.NewGuid(),
        Email = request.Email.ToLower().Trim(),
        First_Name = null,
        Last_Name = null,
        Created_at = DateTime.UtcNow
    };

    db.Users.Add(newUser);
    await db.SaveChangesAsync();

    // Хешируем пароль
    byte[] hash = SHA512.HashData(Encoding.UTF8.GetBytes(request.Password));
    string hex = BitConverter.ToString(hash).Replace("-", "");

    var userPassword = new User_password
    {
        Id = Guid.NewGuid(),
        User_id = newUser.Id,
        Hash_password = hex,
        Created_at = DateTime.UtcNow
    };

    db.UserPasswords.Add(userPassword);
    await db.SaveChangesAsync();

    return Results.Ok(new 
    { 
        Message = "Новый пользователь зарегистрирован",
        UserId = newUser.Id,
        Email = newUser.Email
    });
});

// Проверка всех пользователей и их паролей
app.MapGet("/debug/all-users", async (AppDbContext db) =>
{
    var usersWithPasswords = await db.Users
        .Select(u => new
        {
            User = u,
            Password = db.UserPasswords
                .Where(p => p.User_id == u.Id)
                .Select(p => new { p.Hash_password, p.Created_at })
                .FirstOrDefault()
        })
        .ToListAsync();

    return Results.Ok(usersWithPasswords);
});

app.MapPost("/backup", async (AppDbContext db, BackupDatabaseService backupService) =>
{
    await backupService.CreateBackupAsync(db);
    return Results.Ok("Резевное копирование БД");
});

// регистрации api
app.MapGroup("/channels").MapChannelApi();
app.MapGroup("/groups").MapGroupApi();
app.MapGroup("/group_members").MapGroupMemberApi();
app.MapGroup("/messages").MapMessageApi();
app.MapGroup("/notifications").MapNotificationApi();
app.MapGroup("/problems").MapProblemApi();
app.MapGroup("/users").MapUserApi();
app.MapGroup("/user_passwords").MapUserPasswordApi();
app.MapGroup("/user_profiles").MapUserProfileApi();

app.Run();


public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }

}
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }

}

public class AuthResponse
{
    public string Token { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}