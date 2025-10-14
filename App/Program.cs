using Microsoft.EntityFrameworkCore;
using App.Api;
using Entities;
using System.Drawing;

// база для api
var builder = WebApplication.CreateBuilder(args);

// конфигурация бд(смотри appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// базовые сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// база для приложения
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ¯\_(ツ)_/¯
app.Run(async (context) => await context.Response.SendFileAsync("pupu.jpg"));

// база для api
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

// регистрации api
app.MapGroup("/channels").MapChannelApi();
app.MapGroup("/groups").MapGroupApi();
app.MapGroup("/group_members").MapGroupMemberApi();
app.MapGroup("/messages").MapMessageApi();
app.MapGroup("/notifications").MapNotificationApi();
app.MapGroup("/tasks").MapTaskApi();
app.MapGroup("/task_comments").MapTaskCommentApi();
app.MapGroup("/users").MapUserApi();
app.MapGroup("/user_passwords").MapUserPasswordApi();
app.MapGroup("/user_profiles").MapUserProfileApi();

app.Run();