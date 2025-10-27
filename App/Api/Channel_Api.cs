using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class Channel_Api
    {
        public static RouteGroupBuilder MapChannelApi(this RouteGroupBuilder api)
        {
            // POST - создать канал
            api.MapPost("/", async (Channel channel, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (string.IsNullOrEmpty(channel.Name))
                    return Results.BadRequest("Channel name is required");

                if (channel.Group_id == Guid.Empty)
                    return Results.BadRequest("Group ID is required");

                // Проверка существования группы
                var groupExists = await db.Groups.AnyAsync(g => g.Id == channel.Group_id);
                if (!groupExists)
                    return Results.BadRequest("Group not found");

                // Проверка уникальности имени канала в группе
                var existingChannel = await db.Channels
                    .FirstOrDefaultAsync(c => c.Name == channel.Name && c.Group_id == channel.Group_id);
                if (existingChannel != null)
                    return Results.BadRequest("Channel with this name already exists in the group");

                // Создаем новый канал
                var newChannel = new Channel
                {
                    Id = Guid.NewGuid(),
                    Group_id = channel.Group_id,
                    Name = channel.Name,
                    IsPrivate = channel.IsPrivate
                };

                db.Channels.Add(newChannel);
                await db.SaveChangesAsync();
                return Results.Created($"/channels/{newChannel.Id}", newChannel);
            });

            // GET - получить все каналы
            api.MapGet("/", async (AppDbContext db) => await db.Channels.ToListAsync());

            // GET - получить все каналы группы
            api.MapGet("/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var channels = await db.Channels
                    .Where(c => c.Group_id == groupId)
                    .ToListAsync();
                return Results.Ok(channels);
            });

            // GET - получить один канал по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var channel = await db.Channels.FindAsync(id);
                return channel is null ? Results.NotFound() : Results.Ok(channel);
            });

            // GET - получить канал с данными группы
            api.MapGet("/{id}/with-group", async (Guid id, AppDbContext db) =>
            {
                var result = await (
                    from channel in db.Channels
                    join grp in db.Groups on channel.Group_id equals grp.Id
                    where channel.Id == id
                    select new
                    {
                        Channel = channel,
                        GroupInfo = new
                        {
                            grp.Name,
                            grp.Description
                        }
                    }
                ).FirstOrDefaultAsync();

                return result is null ? Results.NotFound() : Results.Ok(result);
            });

            // GET - получить все публичные каналы группы
            api.MapGet("/group/{groupId}/public", async(Guid groupId, AppDbContext db) =>
            {
                var publicChannels = await db.Channels
                    .Where(c => c.Group_id == groupId && !c.IsPrivate)
                    .ToListAsync();
                return Results.Ok(publicChannels);
            });

            // GET - получить все приватные каналы группы (для администраторов)
            api.MapGet("/group/{groupId}/private", async(Guid groupId, AppDbContext db) =>
            {
                var privateChannels = await db.Channels
                    .Where(c => c.Group_id == groupId && c.IsPrivate)
                    .ToListAsync();
                return Results.Ok(privateChannels);
            });

            // PUT - обновить канал
            api.MapPut("/{id}", async (Guid id, Channel channelData, AppDbContext db) =>
            {
                var channel = await db.Channels.FindAsync(id);
                if (channel is null) return Results.NotFound();

                // Group_id не обновляем - канал нельзя перемещать между группами

                // Проверка уникальности имени канала в группе (если имя изменено)
                if (channel.Name != channelData.Name)
                {
                    var nameExists = await db.Channels
                        .AnyAsync(c => c.Name == channelData.Name &&
                                    c.Group_id == channel.Group_id &&
                                    c.Id != id);
                    if (nameExists)
                        return Results.BadRequest("Channel with this name already exists in the group");
                }

                // Обновляем поля канала
                channel.Name = channelData.Name;
                channel.IsPrivate = channelData.IsPrivate;

                await db.SaveChangesAsync();
                return Results.Ok(channel);
            });

            // DELETE - удалить канал
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var channel = await db.Channels.FindAsync(id);
                if (channel is null) return Results.NotFound();

                db.Channels.Remove(channel);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить все каналы группы
            api.MapDelete("/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var channels = await db.Channels
                    .Where(c => c.Group_id == groupId)
                    .ToListAsync();

                if (!channels.Any())
                    return Results.NotFound("No channels found for this group");

                db.Channels.RemoveRange(channels);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return api;
        }
    }
}