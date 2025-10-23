using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class User_password_Api
    {
        public static RouteGroupBuilder MapUserPasswordApi(this RouteGroupBuilder api)
        {
            // POST - создать/установить пароль пользователя
            api.MapPost("/", async (UserPasswords userPassword, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (userPassword.User_id == Guid.Empty)
                    return Results.BadRequest("User ID is required");

                if (string.IsNullOrEmpty(userPassword.Hash_password))
                    return Results.BadRequest("Password hash is required");

                // Проверка существования пользователя
                var userExists = await db.Users.AnyAsync(u => u.Id == userPassword.User_id);
                if (!userExists)
                    return Results.BadRequest("User not found");

                // Проверка, что у пользователя еще нет пароля
                var existingPassword = await db.UserPasswords
                    .FirstOrDefaultAsync(up => up.User_id == userPassword.User_id);
                if (existingPassword != null)
                    return Results.BadRequest("User already has a password. Use update instead.");

                // Проверка сложности пароля (базовая проверка хеша)
                if (userPassword.Hash_password.Length < 8)
                    return Results.BadRequest("Password hash seems too short");

                // Создаем запись пароля
                var newUserPassword = new UserPasswords
                {
                    Id = Guid.NewGuid(),
                    User_id = userPassword.User_id,
                    Hash_password = userPassword.Hash_password,
                    Created_at = DateTime.UtcNow,
                    Updated_at = null
                };

                db.UserPasswords.Add(newUserPassword);
                await db.SaveChangesAsync();

                // Не возвращаем хеш пароля в ответе
                var response = new
                {
                    newUserPassword.Id,
                    newUserPassword.User_id,
                    newUserPassword.Created_at
                };

                return Results.Created($"/userpasswords/{newUserPassword.Id}", response);
            });

            // GET - получить все записи паролей (только для админов)
            api.MapGet("/", async (AppDbContext db) =>
            {
                var passwords = await db.UserPasswords
                    .Select(up => new
                    {
                        up.Id,
                        up.User_id,
                        up.Created_at,
                        up.Updated_at
                    })
                    .ToListAsync();
                return Results.Ok(passwords);
            });

            // GET - получить запись пароля по ID (без хеша)
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var userPassword = await db.UserPasswords
                    .Where(up => up.Id == id)
                    .Select(up => new
                    {
                        up.Id,
                        up.User_id,
                        up.Created_at,
                        up.Updated_at
                    })
                    .FirstOrDefaultAsync();

                return userPassword is null ? Results.NotFound() : Results.Ok(userPassword);
            });

            // GET - получить запись пароля пользователя (без хеша)
            api.MapGet("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var userPassword = await db.UserPasswords
                    .Where(up => up.User_id == userId)
                    .Select(up => new
                    {
                        up.Id,
                        up.User_id,
                        up.Created_at,
                        up.Updated_at
                    })
                    .FirstOrDefaultAsync();

                return userPassword is null ? Results.NotFound() : Results.Ok(userPassword);
            });

            // POST - проверить пароль (без возврата хеша)
            api.MapPost("/verify", async (VerifyPasswordRequest request, AppDbContext db) =>
            {
                if (request.User_id == Guid.Empty)
                    return Results.BadRequest("User ID is required");

                if (string.IsNullOrEmpty(request.Password_hash))
                    return Results.BadRequest("Password hash is required");

                var userPassword = await db.UserPasswords
                    .FirstOrDefaultAsync(up => up.User_id == request.User_id);

                if (userPassword is null)
                    return Results.NotFound("Password not found for this user");

                // Здесь должна быть реальная проверка пароля
                // В реальном приложении используйте bcrypt или аналоги
                var isPasswordValid = userPassword.Hash_password == request.Password_hash;

                return Results.Ok(new { IsValid = isPasswordValid });
            });

            // PUT - обновить пароль пользователя
            api.MapPut("/{id}", async (Guid id, UserPasswords passwordData, AppDbContext db) =>
            {
                var userPassword = await db.UserPasswords.FindAsync(id);
                if (userPassword is null) return Results.NotFound();

                // Валидация нового пароля
                if (string.IsNullOrEmpty(passwordData.Hash_password))
                    return Results.BadRequest("New password hash is required");

                if (passwordData.Hash_password.Length < 8)
                    return Results.BadRequest("New password hash seems too short");

                // Не позволяем менять User_id
                if (userPassword.User_id != passwordData.User_id)
                    return Results.BadRequest("Cannot change user ID for password");

                // Обновляем пароль
                userPassword.Hash_password = passwordData.Hash_password;
                userPassword.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();

                // Не возвращаем хеш пароля
                var response = new
                {
                    userPassword.Id,
                    userPassword.User_id,
                    userPassword.Created_at,
                    userPassword.Updated_at
                };

                return Results.Ok(response);
            });

            // PATCH - обновить пароль по User_id
            api.MapPatch("/user/{userId}", async (Guid userId, string newPasswordHash, AppDbContext db) =>
            {
                var userPassword = await db.UserPasswords
                    .FirstOrDefaultAsync(up => up.User_id == userId);

                if (userPassword is null)
                    return Results.NotFound("Password not found for this user");

                if (string.IsNullOrEmpty(newPasswordHash))
                    return Results.BadRequest("New password hash is required");

                if (newPasswordHash.Length < 8)
                    return Results.BadRequest("New password hash seems too short");

                userPassword.Hash_password = newPasswordHash;
                userPassword.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();

                var response = new
                {
                    userPassword.Id,
                    userPassword.User_id,
                    userPassword.Created_at,
                    userPassword.Updated_at
                };

                return Results.Ok(response);
            });

            // DELETE - удалить пароль по ID
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var userPassword = await db.UserPasswords.FindAsync(id);
                if (userPassword is null) return Results.NotFound();

                db.UserPasswords.Remove(userPassword);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить пароль пользователя
            api.MapDelete("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var userPassword = await db.UserPasswords
                    .FirstOrDefaultAsync(up => up.User_id == userId);

                if (userPassword is null) return Results.NotFound();

                db.UserPasswords.Remove(userPassword);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // GET - проверить существование пароля пользователя
            api.MapGet("/user/{userId}/exists", async (Guid userId, AppDbContext db) =>
            {
                var hasPassword = await db.UserPasswords
                    .AnyAsync(up => up.User_id == userId);

                return Results.Ok(new { HasPassword = hasPassword });
            });

            // GET - получить историю изменений пароля (если нужно)
            api.MapGet("/user/{userId}/history", async (Guid userId, AppDbContext db) =>
            {
                var passwordHistory = await db.UserPasswords
                    .Where(up => up.User_id == userId)
                    .Select(up => new
                    {
                        up.Id,
                        up.Created_at,
                        up.Updated_at
                    })
                    .OrderByDescending(up => up.Updated_at ?? up.Created_at)
                    .ToListAsync();

                return Results.Ok(passwordHistory);
            });

            return api;
        }

        // DTO для проверки пароля
        public class VerifyPasswordRequest
        {
            public Guid User_id { get; set; }
            public string Password_hash { get; set; }
        }
    }
}