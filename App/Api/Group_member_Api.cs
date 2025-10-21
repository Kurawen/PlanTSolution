using Microsoft.EntityFrameworkCore;
using Entities;

namespace App.Api
{
    public static class Group_member_Api
    {
        public static RouteGroupBuilder MapGroupMemberApi(this RouteGroupBuilder api)
        {
            // POST - добавить участника в группу
            api.MapPost("/", async (Group_member member, AppDbContext db) =>
            {
                // Валидация обязательных полей
                if (member.Group_id == Guid.Empty)
                    return Results.BadRequest("Group ID is required");

                if (member.User_id == Guid.Empty)
                    return Results.BadRequest("User ID is required");

                if (string.IsNullOrEmpty(member.Role))
                    return Results.BadRequest("Role is required");

                // Проверка существования группы
                var groupExists = await db.Groups.AnyAsync(g => g.Id == member.Group_id);
                if (!groupExists)
                    return Results.BadRequest("Group not found");

                // Проверка существования пользователя
                var userExists = await db.Users.AnyAsync(u => u.Id == member.User_id);
                if (!userExists)
                    return Results.BadRequest("User not found");

                // Проверка, что пользователь уже не состоит в группе
                var existingMember = await db.Group_members
                    .FirstOrDefaultAsync(m => m.Group_id == member.Group_id && m.User_id == member.User_id);
                if (existingMember != null)
                    return Results.BadRequest("User is already a member of this group");

                // Валидация роли
                var validRoles = new[] { "member", "admin", "owner" };
                if (!validRoles.Contains(member.Role.ToLower()))
                    return Results.BadRequest("Invalid role. Must be: member, admin, or owner");

                // Создаем нового участника
                var newMember = new Group_member
                {
                    Id = Guid.NewGuid(),
                    Group_id = member.Group_id,
                    User_id = member.User_id,
                    Role = member.Role.ToLower(),
                    Joined_at = DateTime.UtcNow
                };

                db.Group_members.Add(newMember);
                await db.SaveChangesAsync();
                return Results.Created($"/groupmembers/{newMember.Id}", newMember);
            });

            // GET - получить всех участников групп
            api.MapGet("/", async (AppDbContext db) => await db.Group_members.ToListAsync());

            // GET - получить участников конкретной группы
            api.MapGet("/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var members = await db.Group_members
                    .Where(m => m.Group_id == groupId)
                    .ToListAsync();
                return Results.Ok(members);
            });

            // GET - получить группы пользователя
            api.MapGet("/user/{userId}", async (Guid userId, AppDbContext db) =>
            {
                var memberships = await db.Group_members
                    .Where(m => m.User_id == userId)
                    .ToListAsync();
                return Results.Ok(memberships);
            });

            // GET - получить одного участника по ID
            api.MapGet("/{id}", async (Guid id, AppDbContext db) =>
            {
                var member = await db.Group_members.FindAsync(id);
                return member is null ? Results.NotFound() : Results.Ok(member);
            });

            // GET - получить конкретного участника группы
            api.MapGet("/group/{groupId}/user/{userId}", async (Guid groupId, Guid userId, AppDbContext db) =>
            {
                var member = await db.Group_members
                    .FirstOrDefaultAsync(m => m.Group_id == groupId && m.User_id == userId);
                return member is null ? Results.NotFound() : Results.Ok(member);
            });

            // GET - получить администраторов группы
            api.MapGet("/group/{groupId}/admins", async (Guid groupId, AppDbContext db) =>
            {
                var admins = await db.Group_members
                    .Where(m => m.Group_id == groupId && (m.Role == "admin" || m.Role == "owner"))
                    .ToListAsync();
                return Results.Ok(admins);
            });

            // PUT - обновить роль участника
            api.MapPut("/{id}", async (Guid id, Group_member memberData, AppDbContext db) =>
            {
                var member = await db.Group_members.FindAsync(id);
                if (member is null) return Results.NotFound();

                // Валидация роли
                var validRoles = new[] { "member", "admin", "owner" };
                if (!validRoles.Contains(memberData.Role.ToLower()))
                    return Results.BadRequest("Invalid role. Must be: member, admin, or owner");

                // Обновляем только роль (Group_id и User_id не меняем)
                member.Role = memberData.Role.ToLower();

                await db.SaveChangesAsync();
                return Results.Ok(member);
            });

            // PATCH - изменить роль участника
            api.MapPatch("/{id}/role", async (Guid id, string role, AppDbContext db) =>
            {
                var member = await db.Group_members.FindAsync(id);
                if (member is null) return Results.NotFound();

                // Валидация роли
                var validRoles = new[] { "member", "admin", "owner" };
                if (!validRoles.Contains(role.ToLower()))
                    return Results.BadRequest("Invalid role. Must be: member, admin, or owner");

                member.Role = role.ToLower();
                await db.SaveChangesAsync();
                return Results.Ok(member);
            });

            // DELETE - удалить участника из группы
            api.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
            {
                var member = await db.Group_members.FindAsync(id);
                if (member is null) return Results.NotFound();

                db.Group_members.Remove(member);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить пользователя из группы
            api.MapDelete("/group/{groupId}/user/{userId}", async (Guid groupId, Guid userId, AppDbContext db) =>
            {
                var member = await db.Group_members
                    .FirstOrDefaultAsync(m => m.Group_id == groupId && m.User_id == userId);

                if (member is null) return Results.NotFound();

                db.Group_members.Remove(member);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - удалить всех участников группы
            api.MapDelete("/group/{groupId}", async (Guid groupId, AppDbContext db) =>
            {
                var members = await db.Group_members
                    .Where(m => m.Group_id == groupId)
                    .ToListAsync();

                if (!members.Any())
                    return Results.NotFound("No members found in this group");

                db.Group_members.RemoveRange(members);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return api;
        }
    }
}