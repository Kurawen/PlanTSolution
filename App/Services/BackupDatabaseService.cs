using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.IO;
namespace App.Services;

public class BackupDatabaseService
{
    private readonly ILogger<BackupDatabaseService> _logger;

    public BackupDatabaseService(ILogger<BackupDatabaseService> logger)
    {
        _logger = logger;
    }

    public async Task CreateBackupAsync(AppDbContext db)
    {
        var backupDir = "backups";

        var backupFile = $"backup/buckup_{DateTime.Now:yyyyMMdd_HHmmss}.json";
        try
        {
            var backupData = new
            {
                Timestamp = DateTime.UtcNow,
                User = await db.Users.ToListAsync(),
                UserPassword = db.UserPasswords.ToListAsync(),
                Messages = await db.Messages.ToListAsync(),
                Problems = await db.Problem.ToListAsync(),
                Chanels = await db.Channels.ToListAsync(),
                Group = await db.Groups.ToListAsync(),
                GroupMember = await db.GroupMembers.ToListAsync()
            };

            JsonSerializerOptions json = new()
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(json, json);
            await File.WriteAllTextAsync(backupFile, jsonString);

            _logger.LogInformation("Димка создал бэкап БД и назвал его в честь Даты свидания с Анной: {backupFile}", backupFile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Димка не смог создать бэкап [свидания с Фоминой не было:(]");
            throw;
        }
    }
}
