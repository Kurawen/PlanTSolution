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

// база для приложения
var app = builder.Build();

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
app.MapPost("/login/{username}", async (string username, [FromBody] LoginRequest request, AppDbContext db) =>
{
    var user = await
        db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user is null)
        return Results.Unauthorized();
    // Ищем пароль пользователя в отдельной таблице
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
        new(ClaimTypes.Name, user.First_Name),
        new(ClaimTypes.Email, user.Email)
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

    return Results.Ok(new {
        Token = new JwtSecurityTokenHandler().WriteToken(jwt),
        UserId = user.Id,
        Email = user.Email
    }); 
});

app.MapPost("/register", async ([FromBody] RegisterRequest request, AppDbContext db) =>
{
    var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    if (existingUser != null)
        return Results.BadRequest("Пользователь с этим Email уже существует!");

    var newUser = new User
    {
        Email = request.Email,
        Created_at = DateTime.UtcNow
    };

    db.Users.Add(newUser);
    await db.SaveChangesAsync();

    byte[] hash = SHA512.HashData(Encoding.UTF8.GetBytes(request.Password));
    string hex = BitConverter.ToString(hash).Replace("-", "");

    var userPassword = new User_password
    {
        Id = newUser.Id,
        Hash_password = hex,
        Created_at = DateTime.UtcNow
    };

    db.UserPasswords.Add(userPassword);
    await db.SaveChangesAsync();

    return Results.Ok("Новый пользователь зарегистрирован");



});


// app.Map("/data", [Authorize] (HttpContent context) => $"Bebrou");

// регистрации api
app.MapGroup("/channels").MapChannelApi();
app.MapGroup("/groups").MapGroupApi();
app.MapGroup("/group_members").MapGroupMemberApi();
app.MapGroup("/messages").MapMessageApi();
app.MapGroup("/notifications").MapNotificationApi();
app.MapGroup("/problems").MapTaskApi();
app.MapGroup("/users").MapUserApi();
app.MapGroup("/user_passwords").MapUserPasswordApi();
app.MapGroup("/user_profiles").MapUserProfileApi();


// app.Run(async (context) =>
// {
//     app.Logger.LogInformation($"Processing request {context.Request.Path}");

//     await context.Response.WriteAsync("егор пидорас");
// });

// app.Map("/hello", (ILogger<Program> logger) =>
// {
//     logger.LogInformation($"Path: /hello  Time: {DateTime.Now.ToLongTimeString()}");

//     return "piski pipiski";
// });


// провайдеры: 
// дебаг 
// консоль 
// общее для всех
// ошибки

// app.Logger.LogInformation("starting");
// app.MapGet("/", () => "msg");
// app.Logger.LogInformation("ending");

// app.MapGet("/Test", async (ILogger<Program> logger, HttpResponse response) => {
//     logger.LogInformation("testing logging");
//     await response.WriteAsync("bebebe");
// });

// app.Run(async (context) =>
// {
//     var path = context.Request.Path;
//     app.Logger.LogCritical($"LogCritical {path}");
//     app.Logger.LogError($"LogError {path}");
//     app.Logger.LogInformation($"LogInformation {path}");
//     app.Logger.LogWarning($"LogWarning {path}");
 
//     await context.Response.WriteAsync("если ты это читаешь, то все ок");
// });

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