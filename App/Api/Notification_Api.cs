using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class Notification_Api
    {
        public static RouteGroupBuilder MapNotificationApi(this RouteGroupBuilder api)
        {
            // POST - создать уведомление
            api.MapPost("/", async (Notification notification, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (notification.User_id == Guid.Empty)
                    return Results.BadRequest("User ID is required");

                if (notification.Message_id == Guid.Empty)
                    return Results.BadRequest("Message ID is required");

                if (notification.Task_id == Guid.Empty)
                    return Results.BadRequest("Task ID is required");

                if (string.IsNullOrEmpty(notification.Type))
                    return Results.BadRequest("Notification type is required");

                // Проверка существования пользователя
                var userExists = await db.Users.AnyAsync(u => u.Id == notification.User_id);
                if (!userExists)
                    return Results.BadRequest("User not found");

                // Проверка существования сообщения
                var messageExists = await db.Messages.AnyAsync(m => m.Id == notification.Message_id);
                if (!messageExists)
                    return Results.BadRequest("Message not found");

                // Проверка существования задачи
                var taskExists = await db.Tasks.AnyAsync(t => t.Id == notification.Task_id);
                if (!taskExists)
                    return Results.BadRequest("Task not found");

                // Валидация типа уведомления
                var validTypes = new[] { "message", "task_assigned", "task_completed", "task_updated", "mention", "system" };
                if (!validTypes.Contains(notification.Type.ToLower()))
                    return Results.BadRequest("Invalid notification type");

                // Создаем новое уведомление
                var newNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    User_id = notification.User_id,
                    Message_id = notification.Message_id,
                    Task_id = notification.Task_id,
                    Type = notification.Type.ToLower(),
                    Content = notification.Content,
                    Is_read = false,
                    Created_at = DateTime.UtcNow
                };

                db.Notifications.Add(newNotification);
                await db.SaveChangesAsync();
                return Results.Created($"/notifications/{newNotification.Id}", newNotification);
            });

            // GET - получить все уведомления
            api.MapGet("/", async (AppDbContext db) => await db.Notifications.ToListAsync());

            // GET - получить одно уведомление по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var notification = await db.Notifications.FindAsync(id);
                return notification is null ? Results.NotFound() : Results.Ok(notification);
            });

            // GET - получить уведомления пользователя
            api.MapGet("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var notifications = await db.Notifications
                    .Where(n => n.User_id == userId)
                    .OrderByDescending(n => n.Created_at)
                    .ToListAsync();
                return Results.Ok(notifications);
            });

            // GET - получить непрочитанные уведомления пользователя
            api.MapGet("/user/{userId}/unread", async (Guid userId, AppDbContext db) =>
            {
                var unreadNotifications = await db.Notifications
                    .Where(n => n.User_id == userId && !n.Is_read)
                    .OrderByDescending(n => n.Created_at)
                    .ToListAsync();
                return Results.Ok(unreadNotifications);
            });

            // GET - получить уведомления по типу
            api.MapGet("/user/{userId}/type/{type}", async (Guid userId, string type, AppDbContext db) =>
            {
                var notifications = await db.Notifications
                    .Where(n => n.User_id == userId && n.Type == type.ToLower())
                    .OrderByDescending(n => n.Created_at)
                    .ToListAsync();
                return Results.Ok(notifications);
            });

            // GET - получить уведомления с данными сообщений и задач
            api.MapGet("/user/{userId}/with-details", async (Guid userId, AppDbContext db) =>
            {
                var notificationsWithDetails = await (
                    from notification in db.Notifications
                    join message in db.Messages on notification.Message_id equals message.Id
                    join task in db.Tasks on notification.Task_id equals task.Id
                    join user in db.Users on message.User_id equals user.Id
                    where notification.User_id == userId
                    orderby notification.Created_at descending
                    select new
                    {
                        Notification = notification,
                        MessageContent = message.Content,
                        TaskTitle = task.Title,
                        SenderName = user.First_Name + " " + user.Last_Name,
                        SenderEmail = user.Email
                    }
                ).ToListAsync();

                return Results.Ok(notificationsWithDetails);
            });

            // GET - получить количество непрочитанных уведомлений
            api.MapGet("/user/{userId}/unread/count", async (Guid userId, AppDbContext db) =>
            {
                var unreadCount = await db.Notifications
                    .CountAsync(n => n.User_id == userId && !n.Is_read);
                return Results.Ok(new { UnreadCount = unreadCount });
            });

            // PUT - обновить уведомление
            api.MapPut("/{id}", async (Guid id, Notification notificationData, AppDbContext db) =>
            {
                var notification = await db.Notifications.FindAsync(id);
                if (notification is null) return Results.NotFound();

                // User_id, Message_id, Task_id не обновляем - это неизменяемые связи

                // Валидация типа уведомления (если изменен)
                if (notification.Type != notificationData.Type)
                {
                    var validTypes = new[] { "message", "task_assigned", "task_completed", "task_updated", "mention", "system" };
                    if (!validTypes.Contains(notificationData.Type.ToLower()))
                        return Results.BadRequest("Invalid notification type");

                    notification.Type = notificationData.Type.ToLower();
                }

                // Обновляем поля уведомления
                notification.Content = notificationData.Content;
                notification.Is_read = notificationData.Is_read;

                await db.SaveChangesAsync();
                return Results.Ok(notification);
            });

            // PATCH - пометить уведомление как прочитанное
            api.MapPatch("/{id}/read", async (Guid id, AppDbContext db) =>
            {
                var notification = await db.Notifications.FindAsync(id);
                if (notification is null) return Results.NotFound();

                notification.Is_read = true;
                await db.SaveChangesAsync();
                return Results.Ok(notification);
            });

            // PATCH - пометить все уведомления пользователя как прочитанные
            api.MapPatch("/user/{userId}/read-all", async (Guid userId, AppDbContext db) =>
            {
                var notifications = await db.Notifications
                    .Where(n => n.User_id == userId && !n.Is_read)
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.Is_read = true;
                }

                await db.SaveChangesAsync();
                return Results.Ok(new { UpdatedCount = notifications.Count });
            });

            // DELETE - удалить уведомление
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var notification = await db.Notifications.FindAsync(id);
                if (notification is null) return Results.NotFound();

                db.Notifications.Remove(notification);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить все уведомления пользователя
            api.MapDelete("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var notifications = await db.Notifications
                    .Where(n => n.User_id == userId)
                    .ToListAsync();

                if (!notifications.Any())
                    return Results.NotFound("No notifications found for this user");

                db.Notifications.RemoveRange(notifications);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить все прочитанные уведомления пользователя
            api.MapDelete("/user/{userId}/read", async (Guid userId, AppDbContext db) =>
            {
                var readNotifications = await db.Notifications
                    .Where(n => n.User_id == userId && n.Is_read)
                    .ToListAsync();

                if (!readNotifications.Any())
                    return Results.NotFound("No read notifications found for this user");

                db.Notifications.RemoveRange(readNotifications);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // GET - получить статистику уведомлений
            api.MapGet("/user/{userId}/stats", async (Guid userId, AppDbContext db) =>
            {
                var totalNotifications = await db.Notifications
                    .CountAsync(n => n.User_id == userId);

                var unreadNotifications = await db.Notifications
                    .CountAsync(n => n.User_id == userId && !n.Is_read);

                var notificationsByType = await db.Notifications
                    .Where(n => n.User_id == userId)
                    .GroupBy(n => n.Type)
                    .Select(g => new
                    {
                        Type = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var recentActivity = await db.Notifications
                    .Where(n => n.User_id == userId)
                    .OrderByDescending(n => n.Created_at)
                    .Take(5)
                    .ToListAsync();

                return Results.Ok(new
                {
                    TotalNotifications = totalNotifications,
                    UnreadNotifications = unreadNotifications,
                    NotificationsByType = notificationsByType,
                    RecentActivity = recentActivity
                });
            });

            return api;
        }
    }
}