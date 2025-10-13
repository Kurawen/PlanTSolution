using Microsoft.EntityFrameworkCore;
using App;
using Entities;
using System.Drawing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// регистрация обработчика GET-запроса на корневой путь "/"
app.MapGet("/", () => "Bebra tuc-tuc-tuc");

app.Run(async (context) => await context.Response.SendFileAsync("me.jpg"));

// app.MapGet("/", () => "Testing GET");
// app.MapPost("/", () => "Testing POST");
// app.MapPut("/", () => "Testing PUT");
// app.MapDelete("/", () => "Testing DELETE");
// app.MapPatch("/", () => "Testing PATCH");

// app.MapMethods("/", new[] { "OPTIONS" }, () => "Testing OPTIONS");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// видимо подвязка к запуску сборки
// app.MapGroup("/").MapChannelApi();
// app.MapGroup("/").MapGroupApi();
// app.MapGroup("/").MapGroupMemberApi();
// app.MapGroup("/").MapMessageApi();
// app.MapGroup("/").MapNotificationApi();
// app.MapGroup("/").MapTaskApi();
// app.MapGroup("/").MapTaskCommentApi();
// app.MapGroup("/").MapUserApi();
// app.MapGroup("/").MapUserPasswordApi();
// app.MapGroup("/").MapUserProfileApi();

app.Run();