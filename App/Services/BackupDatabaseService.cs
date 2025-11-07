namespace App.Services
{
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
                    User = await db.User.ToListAsync(),
                    UserPassword = db.User_password.ToListAsync(),
                    Messages = await db.Message.ToListAsync(),
                    Problems = await db.Problem.ToListAsync(),
                    Chanels = await db.Chanel.ToListAsync(),
                    Group = await db.Group.ToListAsync(),
                    GroupMember = await db.Group_member.ToListAsync()
                };

                var json = System.Text.Json.JsonSerializer.Serialize(backupData, new System.Text.JsonJsonSerializerOptions
                {
                    WriteIntented = true
                });
                await backupFile.WriteAllTextAsync(backupFile, json);

                _logger.LogInformation("Димка создал бэкап БД и назвал его в честь Даты свидания с Анной: {BacupFile}", backupFile);
            }
            catch (Exception  ex) {
                _logger.LogError(ex, "Димка не смог создать бэкап [свидания с Фоминой не было:(]");
                throw;
        }
    }
}
