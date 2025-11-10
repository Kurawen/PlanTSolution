using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class ProblemApi
    {
        public static RouteGroupBuilder MapProblemApi(this RouteGroupBuilder api)
        {
            // POST - создать задачу
            api.MapPost("/", async (Problem problem, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (string.IsNullOrEmpty(problem.Title))
                    return Results.BadRequest("problem title is required");

                if (problem.Created_by == Guid.Empty)
                    return Results.BadRequest("Creator ID is required");

                if (problem.Assigned_to == Guid.Empty)
                    return Results.BadRequest("Assignee ID is required");

                if (problem.Group_id == Guid.Empty)
                    return Results.BadRequest("Group ID is required");

                // Проверка существования создателя
                var creatorExists = await db.Users.AnyAsync(u => u.Id == problem.Created_by);
                if (!creatorExists)
                    return Results.BadRequest("Creator user not found");

                // Проверка существования исполнителя
                var assigneeExists = await db.Users.AnyAsync(u => u.Id == problem.Assigned_to);
                if (!assigneeExists)
                    return Results.BadRequest("Assignee user not found");

                // Проверка существования группы
                var groupExists = await db.Groups.AnyAsync(g => g.Id == problem.Group_id);
                if (!groupExists)
                    return Results.BadRequest("Group not found");

                // Проверка, что исполнитель состоит в группе
                var isGroupMember = await db.GroupMembers
                    .AnyAsync(m => m.User_id == problem.Assigned_to && m.Group_id == problem.Group_id);
                if (!isGroupMember)
                    return Results.BadRequest("Assignee is not a member of this group");

                // Валидация статуса
                var validStatuses = new[] { "pending", "in_progress", "completed", "cancelled" };
                if (!validStatuses.Contains(problem.Status.ToLower()))
                    return Results.BadRequest("Invalid status. Must be: pending, in_progress, completed, or cancelled");

                // Валидация приоритета
                var validPriorities = new[] { "low", "medium", "high", "urgent" };
                if (!validPriorities.Contains(problem.Priority.ToLower()))
                    return Results.BadRequest("Invalid priority. Must be: low, medium, high, or urgent");

                // Проверка даты выполнения
                if (problem.Due_date <= DateTime.UtcNow)
                    return Results.BadRequest("Due date must be in the future");

                // Создаем новую задачу
                var newProblem = new Problem
                {
                    Id = Guid.NewGuid(),
                    Created_by = problem.Created_by,
                    Assigned_to = problem.Assigned_to,
                    Group_id = problem.Group_id,
                    Title = problem.Title,
                    Description = problem.Description,
                    Status = problem.Status.ToLower(),
                    Priority = problem.Priority.ToLower(),
                    Due_date = problem.Due_date,
                    Created_at = DateTime.UtcNow,
                    Updated_at = DateTime.UtcNow
                };

                db.Problem.Add(newProblem);
                await db.SaveChangesAsync();
                return Results.Created($"/problems/{newProblem.Id}", newProblem);
            });

            // GET - получить все задачи
            api.MapGet("/", async (AppDbContext db) => await db.Problem.ToListAsync());

            // GET - получить одну задачу по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var problem = await db.Problem.FindAsync(id);
                return problem is null ? Results.NotFound() : Results.Ok(problem);
            });

            // GET - получить задачи пользователя (где он исполнитель)
            api.MapGet("/assigned-to/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var problems = await db.Problem
                    .Where(t => t.Assigned_to == userId)
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(problems);
            });

            // GET - получить задачи созданные пользователем
            api.MapGet("/created-by/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var problems = await db.Problem
                    .Where(t => t.Created_by == userId)
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(problems);
            });

            // GET - получить задачи группы
            api.MapGet("/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var problems = await db.Problem
                    .Where(t => t.Group_id == groupId)
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(problems);
            });

            // GET - получить задачи по статусу
            api.MapGet("/status/{status}", async (string status, AppDbContext db) =>
            {
                var problems = await db.Problem
                    .Where(t => t.Status == status.ToLower())
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(problems);
            });

            // GET - получить задачи по приоритету
            api.MapGet("/priority/{priority}", async (string priority, AppDbContext db) =>
            {
                var problems = await db.Problem
                    .Where(t => t.Priority == priority.ToLower())
                    .OrderByDescending(t => t.Created_at)
                    .ToListAsync();
                return Results.Ok(problems);
            });

            // GET - получить просроченные задачи
            api.MapGet("/overdue", async (AppDbContext db) =>
            {
                var overdueProblems = await db.Problem
                    .Where(t => t.Due_date < DateTime.UtcNow && t.Status != "completed" && t.Status != "cancelled")
                    .OrderBy(t => t.Due_date)
                    .ToListAsync();
                return Results.Ok(overdueProblems);
            });

            // GET - получить задачи с деталями пользователей
            api.MapGet("/{id}/with-details", async (Guid id, AppDbContext db) =>
            {
                var problemWithDetails = await (
                    from problem in db.Problem
                    join creator in db.Users on problem.Created_by equals creator.Id
                    join assignee in db.Users on problem.Assigned_to equals assignee.Id
                    join groupObj in db.Groups on problem.Group_id equals groupObj.Id
                    where problem.Id == id
                    select new
                    {
                        problem = problem,
                        Creator = new { creator.First_Name, creator.Last_Name, creator.Email },
                        Assignee = new { assignee.First_Name, assignee.Last_Name, assignee.Email },
                        Group = new { groupObj.Name, groupObj.Description }
                    }
                ).FirstOrDefaultAsync();

                return problemWithDetails is null ? Results.NotFound() : Results.Ok(problemWithDetails);
            });

            // PUT - обновить задачу
            api.MapPut("/{id}", async (Guid id, Problem problemData, AppDbContext db) =>
            {
                var problem = await db.Problem.FindAsync(id);
                if (problem is null) return Results.NotFound();

                // Проверка существования нового исполнителя (если изменен)
                if (problem.Assigned_to != problemData.Assigned_to)
                {
                    var assigneeExists = await db.Users.AnyAsync(u => u.Id == problemData.Assigned_to);
                    if (!assigneeExists)
                        return Results.BadRequest("New assignee user not found");

                    // Проверка, что новый исполнитель состоит в группе
                    var isGroupMember = await db.GroupMembers
                        .AnyAsync(m => m.User_id == problemData.Assigned_to && m.Group_id == problem.Group_id);
                    if (!isGroupMember)
                        return Results.BadRequest("New assignee is not a member of this group");
                }

                // Валидация статуса (если изменен)
                if (problem.Status != problemData.Status)
                {
                    var validStatuses = new[] { "pending", "in_progress", "completed", "cancelled" };
                    if (!validStatuses.Contains(problemData.Status.ToLower()))
                        return Results.BadRequest("Invalid status. Must be: pending, in_progress, completed, or cancelled");
                }

                // Валидация приоритета (если изменен)
                if (problem.Priority != problemData.Priority)
                {
                    var validPriorities = new[] { "low", "medium", "high", "urgent" };
                    if (!validPriorities.Contains(problemData.Priority.ToLower()))
                        return Results.BadRequest("Invalid priority. Must be: low, medium, high, or urgent");
                }

                // Обновляем поля задачи
                problem.Assigned_to = problemData.Assigned_to;
                problem.Title = problemData.Title;
                problem.Description = problemData.Description;
                problem.Status = problemData.Status.ToLower();
                problem.Priority = problemData.Priority.ToLower();
                problem.Due_date = problemData.Due_date;
                problem.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(problem);
            });

            // PATCH - изменить статус задачи
            api.MapPatch("/{id}/status", async (Guid id, string status, AppDbContext db) =>
            {
                var problem = await db.Problem.FindAsync(id);
                if (problem is null) return Results.NotFound();

                var validStatuses = new[] { "pending", "in_progress", "completed", "cancelled" };
                if (!validStatuses.Contains(status.ToLower()))
                    return Results.BadRequest("Invalid status. Must be: pending, in_progress, completed, or cancelled");

                problem.Status = status.ToLower();
                problem.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(problem);
            });

            // PATCH - изменить приоритет задачи
            api.MapPatch("/{id}/priority", async (Guid id, string priority, AppDbContext db) =>
            {
                var problem = await db.Problem.FindAsync(id);
                if (problem is null) return Results.NotFound();

                var validPriorities = new[] { "low", "medium", "high", "urgent" };
                if (!validPriorities.Contains(priority.ToLower()))
                    return Results.BadRequest("Invalid priority. Must be: low, medium, high, or urgent");

                problem.Priority = priority.ToLower();
                problem.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(problem);
            });

            // PATCH - переназначить задачу
            api.MapPatch("/{id}/assign", async (Guid id, Guid newAssigneeId, AppDbContext db) =>
            {
                var problem = await db.Problem.FindAsync(id);
                if (problem is null) return Results.NotFound();

                var assigneeExists = await db.Users.AnyAsync(u => u.Id == newAssigneeId);
                if (!assigneeExists)
                    return Results.BadRequest("New assignee user not found");

                // Проверка, что новый исполнитель состоит в группе
                var isGroupMember = await db.GroupMembers
                    .AnyAsync(m => m.User_id == newAssigneeId && m.Group_id == problem.Group_id);
                if (!isGroupMember)
                    return Results.BadRequest("New assignee is not a member of this group");

                problem.Assigned_to = newAssigneeId;
                problem.Updated_at = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(problem);
            });

            // DELETE - удалить задачу
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var problem = await db.Problem.FindAsync(id);
                if (problem is null) return Results.NotFound();

                db.Problem.Remove(problem);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить все задачи группы
            api.MapDelete("/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var problems = await db.Problem
                    .Where(t => t.Group_id == groupId)
                    .ToListAsync();

                if (!problems.Any())
                    return Results.NotFound("Задачи в этой группе не найдены");

                db.Problem.RemoveRange(problems);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // GET - получить статистику задач
            api.MapGet("/stats/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var totalProblems = await db.Problem
                    .CountAsync(t => t.Group_id == groupId);

                var problemsByStatus = await db.Problem
                    .Where(t => t.Group_id == groupId)
                    .GroupBy(t => t.Status)
                    .Select(g => new
                    {
                        Status = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var problemsByPriority = await db.Problem
                    .Where(t => t.Group_id == groupId)
                    .GroupBy(t => t.Priority)
                    .Select(g => new
                    {
                        Priority = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var overdueProblems = await db.Problem
                    .CountAsync(t => t.Group_id == groupId &&
                                   t.Due_date < DateTime.UtcNow &&
                                   t.Status != "completed" &&
                                   t.Status != "cancelled");

                var topAssignees = await db.Problem
                    .Where(t => t.Group_id == groupId)
                    .GroupBy(t => t.Assigned_to)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        ProblemCount = g.Count()
                    })
                    .OrderByDescending(x => x.ProblemCount)
                    .Take(5)
                    .ToListAsync();

                return Results.Ok(new
                {
                    TotalProblems = totalProblems,
                    ProblemsByStatus = problemsByStatus,
                    ProblemsByPriority = problemsByPriority,
                    OverdueProblems = overdueProblems,
                    TopAssignees = topAssignees
                });
            });

            return api;
        }
    }
}