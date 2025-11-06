using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class GroupApi
    {
        public static RouteGroupBuilder MapGroupApi(this RouteGroupBuilder api)
        {
            // POST - создать группу
            api.MapPost("/", async (Group group, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (string.IsNullOrEmpty(group.Name))
                    return Results.BadRequest("Group name is required");

                if (string.IsNullOrEmpty(group.Description))
                    return Results.BadRequest("Group description is required");

                // Проверка уникальности имени группы
                var existingGroup = await db.Groups.FirstOrDefaultAsync(g => g.Name == group.Name);
                if (existingGroup != null)
                    return Results.BadRequest("Group with this name already exists");

                // Создаем новую группу
                var newGroup = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = group.Name,
                    Description = group.Description,
                    Is_public = group.Is_public,
                    Created_at = DateTime.UtcNow  // Автоматически устанавливаем дату создания
                };

                db.Groups.Add(newGroup);
                await db.SaveChangesAsync();
                return Results.Created($"/groups/{newGroup.Id}", newGroup);
            });

            // GET - получить все группы
            api.MapGet("/", async (AppDbContext db) => await db.Groups.ToListAsync());

            // GET - получить все публичные группы
            api.MapGet("/public", async (AppDbContext db) =>
            {
                var publicGroups = await db.Groups
                    .Where(g => g.Is_public)
                    .ToListAsync();
                return Results.Ok(publicGroups);
            });

            // GET - получить все приватные группы
            api.MapGet("/private", async (AppDbContext db) =>
            {
                var privateGroups = await db.Groups
                    .Where(g => !g.Is_public)
                    .ToListAsync();
                return Results.Ok(privateGroups);
            });

            // GET - получить одну группу по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var group = await db.Groups.FindAsync(id);
                return group is null ? Results.NotFound() : Results.Ok(group);
            });

            // GET - поиск групп по имени
            api.MapGet("/search/{name}", async (string name, AppDbContext db) =>
            {
                var groups = await db.Groups
                    .Where(g => g.Name.Contains(name))
                    .ToListAsync();
                return Results.Ok(groups);
            });

            // PUT - обновить группу
            api.MapPut("/{id}", async (Guid id, Group groupData, AppDbContext db) =>
            {
                var group = await db.Groups.FindAsync(id);
                if (group is null) return Results.NotFound();

                // Проверка уникальности имени (если имя изменено)
                if (group.Name != groupData.Name)
                {
                    var nameExists = await db.Groups.AnyAsync(g => g.Name == groupData.Name && g.Id != id);
                    if (nameExists)
                        return Results.BadRequest("Group with this name already exists");
                }

                // Обновляем поля группы
                group.Name = groupData.Name;
                group.Description = groupData.Description;
                group.Is_public = groupData.Is_public;
                // Created_at не обновляем - это неизменяемое поле

                await db.SaveChangesAsync();
                return Results.Ok(group);
            });

            // PATCH - изменить видимость группы
            api.MapPatch("/{id}/visibility", async (Guid id, bool isPublic, AppDbContext db) =>
            {
                var group = await db.Groups.FindAsync(id);
                if (group is null) return Results.NotFound();

                group.Is_public = isPublic;
                await db.SaveChangesAsync();
                return Results.Ok(group);
            });

            // DELETE - удалить группу
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var group = await db.Groups.FindAsync(id);
                if (group is null) return Results.NotFound();

                // Проверяем, есть ли связанные каналы
                var hasChannels = await db.Channels.AnyAsync(c => c.Group_id == id);
                if (hasChannels)
                    return Results.BadRequest("Cannot delete group with existing channels. Delete channels first.");


                var channels = await db.Channels.Where(c => c.Group_id == id).ToListAsync();
                db.Channels.RemoveRange(channels);

                db.Groups.Remove(group);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить все группы (опасно - только для админов)
            api.MapDelete("/", async (AppDbContext db) =>
            {
                var groups = await db.Groups.ToListAsync();

                if (!groups.Any())
                    return Results.NotFound("No groups found");

                // Проверяем, есть ли группы с каналами
                var groupsWithChannels = await db.Groups
                    .Where(g => db.Channels.Any(c => c.Group_id == g.Id))
                    .ToListAsync();

                if (groupsWithChannels.Any())
                    return Results.BadRequest("Some groups have channels. Delete channels first.");

                db.Groups.RemoveRange(groups);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return api;
        }
    }
}