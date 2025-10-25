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
                if (string.IsNullOrEmpty(user.First_Name))
                    return Results.BadRequest("First name is required");

                if (string.IsNullOrEmpty(user.Last_Name))
                    return Results.BadRequest("Last name is required");

                if (string.IsNullOrEmpty(user.Email))
                    return Results.BadRequest("Email is required");

                // Валидация email формата
                if (!IsValidEmail(user.Email))
                    return Results.BadRequest("Invalid email format");

                // Проверка уникальности email
                var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                    return Results.BadRequest("User with this email already exists");

                // Создаем нового пользователя
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    First_Name = user.First_Name.Trim(),
                    Last_Name = user.Last_Name.Trim(),
                    Email = user.Email.ToLower().Trim(),
                    Created_at = DateTime.UtcNow
                };

                db.Users.Add(newUser);
                await db.SaveChangesAsync();
                return Results.Created($"/users/{newUser.Id}", newUser);
            });

            // GET - получить всех пользователей
            api.MapGet("/", async (AppDbContext db) => await db.Users.ToListAsync());

            // GET - получить одного пользователя по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                return user is null ? Results.NotFound() : Results.Ok(user);
            });

            // GET - получить пользователя по email
            api.MapGet("/email/{email}", async (string email, AppDbContext db) =>
            {
                var user = await db.Users
                    .FirstOrDefaultAsync(u => u.Email == email.ToLower());
                return user is null ? Results.NotFound() : Results.Ok(user);
            });

            // GET - поиск пользователей по имени
            api.MapGet("/search/{name}", async (string name, AppDbContext db) =>
            {
                var users = await db.Users
                    .Where(u => u.First_Name.Contains(name) || u.Last_Name.Contains(name))
                    .ToListAsync();
                return Results.Ok(users);
            });

            // GET - получить пользователей с пагинацией
            api.MapGet("/page/{page}", async (int page, AppDbContext db) =>
            {
                var pageSize = 20;
                var users = await db.Users
                    .OrderBy(u => u.Created_at)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return Results.Ok(users);
            });

            // GET - получить статистику пользователей
            api.MapGet("/stats", async (AppDbContext db) =>
            {
                var totalUsers = await db.Users.CountAsync();
                var todayUsers = await db.Users
                    .CountAsync(u => u.Created_at.Date == DateTime.UtcNow.Date);
                var recentUsers = await db.Users
                    .Where(u => u.Created_at >= DateTime.UtcNow.AddDays(-7))
                    .CountAsync();

                return Results.Ok(new
                {
                    TotalUsers = totalUsers,
                    TodayUsers = todayUsers,
                    RecentUsers = recentUsers
                });
            });

            // PUT - обновить пользователя
            api.MapPut("/{id}", async (Guid id, User userData, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null) return Results.NotFound();

                // Валидация обязательных полей
                if (string.IsNullOrEmpty(userData.First_Name))
                    return Results.BadRequest("First name is required");

                if (string.IsNullOrEmpty(userData.Last_Name))
                    return Results.BadRequest("Last name is required");

                if (string.IsNullOrEmpty(userData.Email))
                    return Results.BadRequest("Email is required");

                // Валидация email формата
                if (!IsValidEmail(userData.Email))
                    return Results.BadRequest("Invalid email format");

                // Проверка уникальности email (если email изменен)
                if (user.Email != userData.Email.ToLower())
                {
                    var emailExists = await db.Users.AnyAsync(u => u.Email == userData.Email.ToLower() && u.Id != id);
                    if (emailExists)
                        return Results.BadRequest("User with this email already exists");
                }

                // Обновляем поля пользователя
                user.First_Name = userData.First_Name.Trim();
                user.Last_Name = userData.Last_Name.Trim();
                user.Email = userData.Email.ToLower().Trim();
                // Created_at не обновляем - это неизменяемое поле

                await db.SaveChangesAsync();
                return Results.Ok(user);
            });

            // PATCH - обновить имя пользователя
            api.MapPatch("/{id}/name", async (Guid id, UpdateUserNameRequest request, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null) return Results.NotFound();

                if (string.IsNullOrEmpty(request.First_Name))
                    return Results.BadRequest("First name is required");

                if (string.IsNullOrEmpty(request.Last_Name))
                    return Results.BadRequest("Last name is required");

                user.First_Name = request.First_Name.Trim();
                user.Last_Name = request.Last_Name.Trim();

                await db.SaveChangesAsync();
                return Results.Ok(user);
            });

            // PATCH - обновить email пользователя
            api.MapPatch("/{id}/email", async (Guid id, string newEmail, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null) return Results.NotFound();

                if (string.IsNullOrEmpty(newEmail))
                    return Results.BadRequest("Email is required");

                if (!IsValidEmail(newEmail))
                    return Results.BadRequest("Invalid email format");

                // Проверка уникальности email
                var emailExists = await db.Users.AnyAsync(u => u.Email == newEmail.ToLower() && u.Id != id);
                if (emailExists)
                    return Results.BadRequest("User with this email already exists");

                user.Email = newEmail.ToLower().Trim();
                await db.SaveChangesAsync();
                return Results.Ok(user);
            });

            // DELETE - удалить пользователя
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null) return Results.NotFound();

                // Проверяем связанные данные перед удалением
                var hasProfile = await db.UserProfiles.AnyAsync(p => p.User_id == id);
                var hasMessages = await db.Messages.AnyAsync(m => m.User_id == id);
                var hasGroupMemberships = await db.GroupMembers.AnyAsync(gm => gm.User_id == id);
                var hasTasksCreated = await db.Problem.AnyAsync(t => t.Created_by == id);
                var hasTasksAssigned = await db.Problem.AnyAsync(t => t.Assigned_to == id);
                var hasPassword = await db.UserPasswords.AnyAsync(up => up.User_id == id);

                if (hasProfile || hasMessages || hasGroupMemberships || hasTasksCreated || hasTasksAssigned || hasPassword)
                {
                    return Results.BadRequest("Cannot delete user with existing related data. Delete related data first.");
                }

                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить всех пользователей (опасно - только для админов)
            api.MapDelete("/", async (AppDbContext db) =>
            {
                var users = await db.Users.ToListAsync();

                if (!users.Any())
                    return Results.NotFound("No users found");

                // Проверяем, есть ли пользователи с связанными данными
                var usersWithRelations = await db.Users
                    .Where(u => db.UserProfiles.Any(p => p.User_id == u.Id) ||
                               db.Messages.Any(m => m.User_id == u.Id) ||
                               db.GroupMembers.Any(gm => gm.User_id == u.Id) ||
                               db.Problem.Any(t => t.Created_by == u.Id) ||
                               db.Problem.Any(t => t.Assigned_to == u.Id) ||
                               db.UserPasswords.Any(up => up.User_id == u.Id))
                    .ToListAsync();

                if (usersWithRelations.Any())
                    return Results.BadRequest("Some users have related data. Delete related data first.");

                db.Users.RemoveRange(users);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // GET - проверить существование пользователя по email
            api.MapGet("/exists/{email}", async (string email, AppDbContext db) =>
            {
                var exists = await db.Users.AnyAsync(u => u.Email == email.ToLower());
                return Results.Ok(new { Exists = exists });
            });

            return api;
        }

        // Вспомогательный метод для валидации email
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // DTO для обновления имени
        public class UpdateUserNameRequest
        {
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
        }
    }
}