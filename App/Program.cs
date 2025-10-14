using Microsoft.EntityFrameworkCore;
using App;
using Entities;
using System.Drawing;

// база для api
var builder = WebApplication.CreateBuilder(args);

// базовые сервисы
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
app.Run(async (context) => await context.Response.SendFileAsync("me.jpg"));

// база для api
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();

// регистрация обработчика GET-запроса на корневой путь "/"
// видимо подвязка к запуску сборки
// app.MapGroup("/").MapChannelApi();
// app.MapGroup("/").MapGroupApi();
// app.MapGroup("/").MapGroupMemberApi();
// app.MapGroup("/").MapMessageApi();
// app.MapGroup("/").MapNotificationApi();
// app.MapGroup("/").MapTaskApi();
// app.MapGroup("/").MapTaskCommentApi();
app.MapGroup("/user").MapUserApi();
// app.MapGroup("/").MapUserPasswordApi();
// app.MapGroup("/").MapUserProfileApi();

app.Run();