using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.IO;
namespace App.Services;

public class BackupDatabaseService
{
    private readonly ILogger<BackupDatabaseService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public BackupDatabaseService(ILogger<BackupDatabaseService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task<string> CreateBackupAsync(AppDbContext db)
    {
        using var scope = _scopeFactory.CreateScope();
        var dub = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var backupDir = "backups";
        if (!Directory.Exists(backupDir))
        {
            Directory.CreateDirectory(backupDir);
        }

        var backupFile = $"backups/buckup_{DateTime.Now:yyyyMMdd_HHmmss}.json";

        try
        {
            var backupData = new
            {
                Timestamp = DateTime.UtcNow,
                User = await db.Users.ToListAsync(),
                UserPassword = await db.UserPasswords.ToListAsync(),
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
            string jsonString = JsonSerializer.Serialize(backupData, json);
            await File.WriteAllTextAsync(backupFile, jsonString, Encoding.UTF8);

            _logger.LogInformation("Димка создал бэкап БД и назвал его в честь Даты свидания с Анной: {backupFile}", backupFile);

            return backupFile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Димка не смог создать бэкап [свидания с Фоминой не было:(]");
            throw;
        }
    }
}
