using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class User_profile_Api
    {
        public static RouteGroupBuilder MapUserProfileApi(this RouteGroupBuilder api)
        {
            // POST - создать профиль пользователя
            api.MapPost("/", async (User_profile profile, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (profile.User_id == Guid.Empty)
                    return Results.BadRequest("User ID is required");

                // Проверка существования пользователя
                var userExists = await db.Users.AnyAsync(u => u.Id == profile.User_id);
                if (!userExists)
                    return Results.BadRequest("User not found");

                // Проверка, что профиль для этого пользователя еще не существует
                var existingProfile = await db.User_profiles
                    .FirstOrDefaultAsync(p => p.User_id == profile.User_id);
                if (existingProfile != null)
                    return Results.BadRequest("Profile for this user already exists");

                // Проверка уникальности номера телефона (если указан)
                if (!string.IsNullOrEmpty(profile.Phone_number))
                {
                    var phoneExists = await db.User_profiles
                        .AnyAsync(p => p.Phone_number == profile.Phone_number);
                    if (phoneExists)
                        return Results.BadRequest("User with this phone number already exists");
                }

                // Создаем новый профиль
                var newProfile = new User_profile
                {
                    Id = Guid.NewGuid(),
                    User_id = profile.User_id,
                    Phone_number = profile.Phone_number,
                    Avatar_url = profile.Avatar_url
                };

                db.User_profiles.Add(newProfile);
                await db.SaveChangesAsync();
                return Results.Created($"/userprofiles/{newProfile.Id}", newProfile);
            });

            // GET - получить все профили пользователей
            api.MapGet("/", async (AppDbContext db) =>
                await db.User_profiles.ToListAsync());

            // GET - получить один профиль по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var profile = await db.User_profiles.FindAsync(id);
                return profile is null ? Results.NotFound() : Results.Ok(profile);
            });

            // GET - получить профиль по ID пользователя
            api.MapGet("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var profile = await db.User_profiles
                    .FirstOrDefaultAsync(p => p.User_id == userId);

                return profile is null ? Results.NotFound() : Results.Ok(profile);
            });

            // GET - получить профиль с данными пользователя (расширенный вариант)
            api.MapGet("/{id}/with-user", async (Guid id, AppDbContext db) =>
            {
                var result = await (
                    from profile in db.User_profiles
                    join user in db.Users on profile.User_id equals user.Id
                    where profile.Id == id
                    select new
                    {
                        Profile = profile,
                        UserInfo = new
                        {
                            user.First_Name,
                            user.Last_Name,
                            user.Email,
                            user.Created_at
                        }
                    }
                ).FirstOrDefaultAsync();

                return result is null ? Results.NotFound() : Results.Ok(result);
            });

            // GET - получить все профили с данными пользователей
            api.MapGet("/with-users", async (AppDbContext db) =>
            {
                var results = await (
                    from profile in db.User_profiles
                    join user in db.Users on profile.User_id equals user.Id
                    select new
                    {
                        Profile = profile,
                        UserInfo = new
                        {
                            user.First_Name,
                            user.Last_Name,
                            user.Email
                        }
                    }
                ).ToListAsync();

                return Results.Ok(results);
            });

            // PUT - обновить профиль пользователя
            api.MapPut("/{id}", async (Guid id, User_profile profileData, AppDbContext db) =>
            {
                var profile = await db.User_profiles.FindAsync(id);
                if (profile is null) return Results.NotFound();

                // User_id не обновляем - это неизменяемая связь
                // Проверка уникальности номера телефона (если изменен и указан)
                if (!string.IsNullOrEmpty(profileData.Phone_number) &&
                    profile.Phone_number != profileData.Phone_number)
                {
                    var phoneExists = await db.User_profiles
                        .AnyAsync(p => p.Phone_number == profileData.Phone_number && p.Id != id);
                    if (phoneExists)
                        return Results.BadRequest("User with this phone number already exists");
                }

                // Обновляем поля профиля
                profile.Phone_number = profileData.Phone_number;
                profile.Avatar_url = profileData.Avatar_url;

                await db.SaveChangesAsync();
                return Results.Ok(profile);
            });

            // DELETE - удалить профиль пользователя
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var profile = await db.User_profiles.FindAsync(id);
                if (profile is null) return Results.NotFound();

                db.User_profiles.Remove(profile);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить профиль по ID пользователя
            api.MapDelete("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var profile = await db.User_profiles
                    .FirstOrDefaultAsync(p => p.User_id == userId);

                if (profile is null) return Results.NotFound();

                db.User_profiles.Remove(profile);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return api;
        }
    }
}