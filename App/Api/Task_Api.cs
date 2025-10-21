using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class Task_Api
    {
        public static RouteGroupBuilder MapTaskApi(this RouteGroupBuilder api)
        {
            // POST - создать задачу
            api.MapPost("/", async (Task task, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (string.IsNullOrEmpty(task.Title))
                    return Results.BadRequest("Task title is required");

                if (task.Created_by == Guid.Empty)
                    return Results.BadRequest("Creator ID is required");

                if (task.Assigned_to == Guid.Empty)
                    return Results.BadRequest("Assignee ID is required");

                if (task.Group_id == Guid.Empty)
                    return Results.BadRequest("Group ID is required");

                // Проверка существования создателя
                var creatorExists = await db.Users.AnyAsync(u => u.Id == task.Created_by);
                if (!creatorExists)
                    return Results.BadRequest("Creator user not found");

                // Проверка существования исполнителя
                var assigneeExists = await db.Users.AnyAsync(u => u.Id == task.Assigned_to);
                if (!assigneeExists)
                    return Results.BadRequest("Assignee user not found");

                // Проверка существования группы
                var groupExists = await db.Groups.AnyAsync(g => g.Id == task.Group_id);
                if (!groupExists)
                    return Results.BadRequest("Group not found");

                // Проверка, что исполнитель состоит в группе
                var isGroupMember = await db.Group_members
                    .AnyAsync(m => m.User_id == task.Assigned_to && m.Group_id == task.Group_id);
                if (!isGroupMember)
                    return Results.BadRequest("Assignee is not a member of this group");

                // Валидация статуса
                var validStatuses = new[] { "pending", "in_progress", "completed", "cancelled" };
                if (!validStatuses.Contains(task.Status.ToLower()))
                    return Results.BadRequest("Invalid status. Must be: pending, in_progress, completed, or cancelled");

                // Валидация приоритета
                var validPriorities = new[] { "low", "medium", "high", "urgent" };
                if (!validPriorities.Contains(task.Priority.ToLower()))
                    return Results.BadRequest("Invalid priority. Must be: low, medium, high, or urgent");

                // Проверка даты выполнения
                if (task.Due_date <= DateTime.UtcNow)
                    return Results.BadRequest("Due date must be in the future");

                // Создаем новую задачу
                var newTask = new Task
                {
                    Id = Guid.NewGuid(),
                    Created_by = task.Created_by,
                    Assigned_to = task.Assigned_to,
                    Group_id = task.Group_id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status.ToLower(),
                    Priority = task.Priority.ToLower(),
                    Due_date = task.Due_date,
                    Created_at = DateTime.UtcNow,
                    Updated_at = DateTime.UtcNow
                };

                db.Tasks.Add(newTask);
                await db.SaveChangesAsync();
                return Results.Created($"/tasks/{newTask.Id}", newTask);
            });

            // GET - получить все задачи
            api.MapGet("/", async (AppDbContext db) => await db.Tasks.ToListAsync());

            // GET - получить одну задачу по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var task = await db.Tasks.FindAsync(id);
                return task is null ? Results.NotFound() : Results.Ok(task);
            });

            // GET - получить задачи пользователя (где он исполнитель)
            api.MapGet("/assigned-to/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var tasks = await db.Tasks
                    .Where(t => t.Assigned_to == userId)
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(tasks);
            });

            // GET - получить задачи созданные пользователем
            api.MapGet("/created-by/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var tasks = await db.Tasks
                    .Where(t => t.Created_by == userId)
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(tasks);
            });

            // GET - получить задачи группы
            api.MapGet("/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var tasks = await db.Tasks
                    .Where(t => t.Group_id == groupId)
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(tasks);
            });

            // GET - получить задачи по статусу
            api.MapGet("/status/{status}", async (string status, AppDbContext db) =>
            {
                var tasks = await db.Tasks
                    .Where(t => t.Status == status.ToLower())
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(tasks);
            });

            // GET - получить задачи по приоритету
            api.MapGet("/priority/{priority}", async (string priority, AppDbContext db) =>
            {
                var tasks = await db.Tasks
                    .Where(t => t.Priority == priority.ToLower())
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(tasks);
            });

            // GET - получить просроченные задачи
            api.MapGet("/overdue", async (AppDbContext db) =>
            {
                var overdueTasks = await db.Tasks
                    .Where(t => t.Due_date < DateTime.UtcNow && t.Status != "completed" && t.Status != "cancelled")
                    .OrderBy(t => t.Due_date)
                    .ToListAsync();
                return Results.Ok(overdueTasks);
            });

            // GET - получить задачи с деталями пользователей
            api.MapGet("/{id}/with-details", async (Guid id, AppDbContext db) =>
            {
                var taskWithDetails = await (
                    from task in db.Tasks
                    join creator in db.Users on task.Created_by equals creator.Id
                    join assignee in db.Users on task.Assigned_to equals assignee.Id
                    join groupObj in db.Groups on task.Group_id equals groupObj.Id
                    where task.Id == id
                    select new
                    {
                        Task = task,
                        Creator = new { creator.First_Name, creator.Last_Name, creator.Email },
                        Assignee = new { assignee.First_Name, assignee.Last_Name, assignee.Email },
                        Group = new { groupObj.Name, groupObj.Description }
                    }
                ).FirstOrDefaultAsync();

                return taskWithDetails is null ? Results.NotFound() : Results.Ok(taskWithDetails);
            });

            // PUT - обновить задачу
            api.MapPut("/{id}", async (Guid id, Task taskData, AppDbContext db) =>
            {
                var task = await db.Tasks.FindAsync(id);
                if (task is null) return Results.NotFound();

                // Проверка существования нового исполнителя (если изменен)
                if (task.Assigned_to != taskData.Assigned_to)
                {
                    var assigneeExists = await db.Users.AnyAsync(u => u.Id == taskData.Assigned_to);
                    if (!assigneeExists)
                        return Results.BadRequest("New assignee user not found");

                    // Проверка, что новый исполнитель состоит в группе
                    var isGroupMember = await db.Group_members
                        .AnyAsync(m => m.User_id == taskData.Assigned_to && m.Group_id == task.Group_id);
                    if (!isGroupMember)
                        return Results.BadRequest("New assignee is not a member of this group");
                }

                // Валидация статуса (если изменен)
                if (task.Status != taskData.Status)
                {
                    var validStatuses = new[] { "pending", "in_progress", "completed", "cancelled" };
                    if (!validStatuses.Contains(taskData.Status.ToLower()))
                        return Results.BadRequest("Invalid status. Must be: pending, in_progress, completed, or cancelled");
                }

                // Валидация приоритета (если изменен)
                if (task.Priority != taskData.Priority)
                {
                    var validPriorities = new[] { "low", "medium", "high", "urgent" };
                    if (!validPriorities.Contains(taskData.Priority.ToLower()))
                        return Results.BadRequest("Invalid priority. Must be: low, medium, high, or urgent");
                }

                // Обновляем поля задачи
                task.Assigned_to = taskData.Assigned_to;
                task.Title = taskData.Title;
                task.Description = taskData.Description;
                task.Status = taskData.Status.ToLower();
                task.Priority = taskData.Priority.ToLower();
                task.Due_date = taskData.Due_date;
                task.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(task);
            });

            // PATCH - изменить статус задачи
            api.MapPatch("/{id}/status", async (Guid id, string status, AppDbContext db) =>
            {
                var task = await db.Tasks.FindAsync(id);
                if (task is null) return Results.NotFound();

                var validStatuses = new[] { "pending", "in_progress", "completed", "cancelled" };
                if (!validStatuses.Contains(status.ToLower()))
                    return Results.BadRequest("Invalid status. Must be: pending, in_progress, completed, or cancelled");

                task.Status = status.ToLower();
                task.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(task);
            });

            // PATCH - изменить приоритет задачи
            api.MapPatch("/{id}/priority", async (Guid id, string priority, AppDbContext db) =>
            {
                var task = await db.Tasks.FindAsync(id);
                if (task is null) return Results.NotFound();

                var validPriorities = new[] { "low", "medium", "high", "urgent" };
                if (!validPriorities.Contains(priority.ToLower()))
                    return Results.BadRequest("Invalid priority. Must be: low, medium, high, or urgent");

                task.Priority = priority.ToLower();
                task.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(task);
            });

            // PATCH - переназначить задачу
            api.MapPatch("/{id}/assign", async (Guid id, Guid newAssigneeId, AppDbContext db) =>
            {
                var task = await db.Tasks.FindAsync(id);
                if (task is null) return Results.NotFound();

                var assigneeExists = await db.Users.AnyAsync(u => u.Id == newAssigneeId);
                if (!assigneeExists)
                    return Results.BadRequest("New assignee user not found");

                // Проверка, что новый исполнитель состоит в группе
                var isGroupMember = await db.Group_members
                    .AnyAsync(m => m.User_id == newAssigneeId && m.Group_id == task.Group_id);
                if (!isGroupMember)
                    return Results.BadRequest("New assignee is not a member of this group");

                task.Assigned_to = newAssigneeId;
                task.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(task);
            });

            // DELETE - удалить задачу
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var task = await db.Tasks.FindAsync(id);
                if (task is null) return Results.NotFound();

                db.Tasks.Remove(task);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить все задачи группы
            api.MapDelete("/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var tasks = await db.Tasks
                    .Where(t => t.Group_id == groupId)
                    .ToListAsync();

                if (!tasks.Any())
                    return Results.NotFound("No tasks found for this group");

                db.Tasks.RemoveRange(tasks);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // GET - получить статистику задач
            api.MapGet("/stats/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var totalTasks = await db.Tasks
                    .CountAsync(t => t.Group_id == groupId);

                var tasksByStatus = await db.Tasks
                    .Where(t => t.Group_id == groupId)
                    .GroupBy(t => t.Status)
                    .Select(g => new
                    {
                        Status = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var tasksByPriority = await db.Tasks
                    .Where(t => t.Group_id == groupId)
                    .GroupBy(t => t.Priority)
                    .Select(g => new
                    {
                        Priority = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var overdueTasks = await db.Tasks
                    .CountAsync(t => t.Group_id == groupId &&
                                   t.Due_date < DateTime.UtcNow &&
                                   t.Status != "completed" &&
                                   t.Status != "cancelled");

                var topAssignees = await db.Tasks
                    .Where(t => t.Group_id == groupId)
                    .GroupBy(t => t.Assigned_to)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        TaskCount = g.Count()
                    })
                    .OrderByDescending(x => x.TaskCount)
                    .Take(5)
                    .ToListAsync();

                return Results.Ok(new
                {
                    TotalTasks = totalTasks,
                    TasksByStatus = tasksByStatus,
                    TasksByPriority = tasksByPriority,
                    OverdueTasks = overdueTasks,
                    TopAssignees = topAssignees
                });
            });

            return api;
        }
    }
}