using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class Message_Api
    {
        public static RouteGroupBuilder MapMessageApi(this RouteGroupBuilder api)
        {
            // POST - создать сообщение
            api.MapPost("/", async (Message message, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (string.IsNullOrEmpty(message.Content))
                    return Results.BadRequest("Message content is required");

                if (message.User_id == Guid.Empty)
                    return Results.BadRequest("User ID is required");

                if (message.Channel_id == Guid.Empty)
                    return Results.BadRequest("Channel ID is required");

                // Проверка существования пользователя
                var userExists = await db.Users.AnyAsync(u => u.Id == message.User_id);
                if (!userExists)
                    return Results.BadRequest("User not found");

                // Проверка существования канала
                var channelExists = await db.Channels.AnyAsync(c => c.Id == message.Channel_id);
                if (!channelExists)
                    return Results.BadRequest("Channel not found");

                // Проверка, что пользователь состоит в группе канала
                var channel = await db.Channels.FindAsync(message.Channel_id);
                var isGroupMember = await db.GroupMembers
                    .AnyAsync(m => m.User_id == message.User_id && m.Group_id == channel.Group_id);

                if (!isGroupMember)
                    return Results.BadRequest("User is not a member of this group");

                // Создаем новое сообщение
                var newMessage = new Message
                {
                    Id = Guid.NewGuid(),
                    User_id = message.User_id,
                    Channel_id = message.Channel_id,
                    Content = message.Content,
                    Sent_at = DateTime.UtcNow
                };

                db.Messages.Add(newMessage);
                await db.SaveChangesAsync();
                return Results.Created($"/messages/{newMessage.Id}", newMessage);
            });

            // GET - получить все сообщения
            api.MapGet("/", async (AppDbContext db) => await db.Messages.ToListAsync());

            // GET - получить одно сообщение по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var message = await db.Messages.FindAsync(id);
                return message is null ? Results.NotFound() : Results.Ok(message);
            });

            // GET - получить сообщения канала
            api.MapGet("/channel/{channelId}", async (Guid channelId, AppDbContext db) =>
            {
                var messages = await db.Messages
                    .Where(m => m.Channel_id == channelId)
                    .OrderBy(m => m.Sent_at)
                    .ToListAsync();
                return Results.Ok(messages);
            });

            // GET - получить сообщения канала с пагинацией
            api.MapGet("/channel/{channelId}/page/{page}", async (Guid channelId, int page, AppDbContext db) =>
            {
                var pageSize = 50;
                var messages = await db.Messages
                    .Where(m => m.Channel_id == channelId)
                    .OrderByDescending(m => m.Sent_at)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return Results.Ok(messages);
            });

            // GET - получить сообщения пользователя
            api.MapGet("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var messages = await db.Messages
                    .Where(m => m.User_id == userId)
                    .OrderByDescending(m => m.Sent_at)
                    .ToListAsync();
                return Results.Ok(messages);
            });

            // GET - получить сообщения с данными пользователя
            api.MapGet("/channel/{channelId}/with-users", async (Guid channelId, AppDbContext db) =>
            {
                var messagesWithUsers = await (
                    from message in db.Messages
                    join user in db.Users on message.User_id equals user.Id
                    where message.Channel_id == channelId
                    orderby message.Sent_at
                    select new
                    {
                        Message = message,
                        UserInfo = new
                        {
                            user.First_Name,
                            user.Last_Name,
                            user.Email
                        }
                    }
                ).ToListAsync();

                return Results.Ok(messagesWithUsers);
            });

            // GET - поиск сообщений по содержанию
            api.MapGet("/search/{query}", async (string query, AppDbContext db) =>
            {
                var messages = await db.Messages
                    .Where(m => m.Content.Contains(query))
                    .OrderByDescending(m => m.Sent_at)
                    .ToListAsync();
                return Results.Ok(messages);
            });

            // GET - поиск сообщений в канале
            api.MapGet("/channel/{channelId}/search/{query}", async (Guid channelId, string query, AppDbContext db) =>
            {
                var messages = await db.Messages
                    .Where(m => m.Channel_id == channelId && m.Content.Contains(query))
                    .OrderByDescending(m => m.Sent_at)
                    .ToListAsync();
                return Results.Ok(messages);
            });

            // PUT - обновить сообщение
            api.MapPut("/{id}", async (Guid id, Message messageData, AppDbContext db) =>
            {
                var message = await db.Messages.FindAsync(id);
                if (message is null) return Results.NotFound();

                // Проверяем, что прошло не больше 5 минут с момента отправки
                var timeDiff = DateTime.UtcNow - message.Sent_at;
                if (timeDiff.TotalMinutes > 5)
                    return Results.BadRequest("Message can only be edited within 5 minutes of sending");

                // Обновляем только содержание (User_id, Channel_id, Sent_at не меняем)
                message.Content = messageData.Content;

                await db.SaveChangesAsync();
                return Results.Ok(message);
            });

            // DELETE - удалить сообщение
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var message = await db.Messages.FindAsync(id);
                if (message is null) return Results.NotFound();

                db.Messages.Remove(message);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить все сообщения канала
            api.MapDelete("/channel/{channelId}", async (Guid channelId, AppDbContext db) =>
            {
                var messages = await db.Messages
                    .Where(m => m.Channel_id == channelId)
                    .ToListAsync();

                if (!messages.Any())
                    return Results.NotFound("No messages found in this channel");

                db.Messages.RemoveRange(messages);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить все сообщения пользователя
            api.MapDelete("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var messages = await db.Messages
                    .Where(m => m.User_id == userId)
                    .ToListAsync();

                if (!messages.Any())
                    return Results.NotFound("No messages found for this user");

                db.Messages.RemoveRange(messages);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // GET - получить статистику сообщений
            api.MapGet("/stats/channel/{channelId}", async (Guid channelId, AppDbContext db) =>
            {
                var totalMessages = await db.Messages
                    .CountAsync(m => m.Channel_id == channelId);

                var todayMessages = await db.Messages
                    .CountAsync(m => m.Channel_id == channelId &&
                                   m.Sent_at.Date == DateTime.UtcNow.Date);

                var topPosters = await db.Messages
                    .Where(m => m.Channel_id == channelId)
                    .GroupBy(m => m.User_id)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        MessageCount = g.Count()
                    })
                    .OrderByDescending(x => x.MessageCount)
                    .Take(10)
                    .ToListAsync();

                return Results.Ok(new
                {
                    TotalMessages = totalMessages,
                    TodayMessages = todayMessages,
                    TopPosters = topPosters
                });
            });

            return api;
        }
    }
}