using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class User_Api
    {
        public static RouteGroupBuilder MapUserApi(this RouteGroupBuilder api)
        {
            // POST - создать пользователя
            api.MapPost("/", async (User user, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (string.IsNullOrEmpty(user.Email))
                    return Results.BadRequest("Email is required");

                // Проверка уникальности email
                var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                    return Results.BadRequest("User with this email already exists");

                // Создаем нового пользователя
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    First_Name = user.First_Name,
                    Last_Name = user.Last_Name,
                    Email = user.Email,
                    Created_at = DateTime.UtcNow  // Автоматически устанавливаем дату создания
                };

                db.Users.Add(newUser);
                await db.SaveChangesAsync();
                return Results.Created($"/users/{newUser.Id}", newUser);
            });

            // GET - получить всех пользователей
            api.MapGet("/", async (AppDbContext db) => await db.Users.ToListAsync());

            // GET - получить одного пользователя
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                // получаем пользователя по id
                var user = await db.Users.FindAsync(id);
            
            
                // найден или не найден, вот в чем вопрос
                return user is null ? Results.NotFound() : Results.Ok(user);
            });
            
            // PUT - обновить пользователя
            api.MapPut("/{id}", async (Guid id, User userData, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null) return Results.NotFound();

                // Проверка уникальности email (если email изменен)
                if (user.Email != userData.Email)
                {
                    var emailExists = await db.Users.AnyAsync(u => u.Email == userData.Email && u.Id != id);
                    if (emailExists)
                        return Results.BadRequest("User with this email already exists");
                }

                // Обновляем поля пользователя
                user.First_Name = userData.First_Name;
                user.Last_Name = userData.Last_Name;
                user.Email = userData.Email;
                // Created_at не обновляем - это неизменяемое поле

                await db.SaveChangesAsync();
                return Results.Ok(user);
            });

            // DELETE - удалить пользователя
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                // получаем пользователя по id
                var user = await db.Users.FindAsync(id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (user is null) return Results.NotFound();

                // если пользователь найден, удаляем его
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return api;
        }
    }
}