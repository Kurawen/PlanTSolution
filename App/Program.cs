using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Drawing;
using System.Text;

using App.Api;
using Entities;

// база для api
var builder = WebApplication.CreateBuilder(args);

// конфигурация бд(смотри appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// базовые сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddAuthorization();
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(FileOptions =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidIssuer = AuthOptions.ISSUER,
//             ValidateAudience = true,
//             ValidAudience = AuthOptions.AUDIENCE,
//             ValidateLifetime = true,
//             IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
//             ValidateIssuerSigningKey = true,
//         };
//     });

// база для приложения
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// app.Map("/login/{username}", (string username) =>
// {
//     var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

//     var jwt = new JwtSecurityToken(
//         issuer: AuthOptions.ISSUER,
//         audience: AuthOptions.AUDIENCE,
//         claims: claims,
//         expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
//         signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

//     return new JwtSecurityTokenHandler().WriteToken(jwt);
// });

// app.Map("/data", [Authorize] () => new { message = "тест тест тест" });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ¯\_(ツ)_/¯ - не трогать
// app.Run(async (context) => await context.Response.SendFileAsync("pupu.jpg"));

// база для api
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

// регистрации api
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