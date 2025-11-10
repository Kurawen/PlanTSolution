using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging; // Добавляем логгер

namespace App.Services;

public class CacheService
{
    private readonly AppDbContext _db;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(AppDbContext context, IMemoryCache memoryCache, ILogger<CacheService> logger)
    {
        _db = context;
        _cache = memoryCache;
        _logger = logger;
    }

    public async Task<User?> GetUser(Guid id)
    {
        // Пытаемся получить данные из кэша
        if (_cache.TryGetValue(id, out User? user))
        {
            _logger.LogInformation("Пользователь {Email} извлечен из кэша", user?.Email);
            return user;
        }

        // Если данные не найдены в кэше - обращаемся к базе данных
        user = await _db.Users.FindAsync(id);
        
        if (user != null)
        {
            _logger.LogInformation("Пользователь {Email} извлечен из базы данных", user.Email);

            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                Priority = CacheItemPriority.Normal,
            };

            cacheOptions.RegisterPostEvictionCallback(
                (key, value, reason, state) => 
                {
                    _logger.LogInformation("Запись {Key} удалена из кэша. Причина: {Reason}", key, reason);
                });

            _cache.Set(user.Id, user, cacheOptions);
        }
        else
        {
            _logger.LogWarning("Пользователь с ID {UserId} не найден в базе данных", id);
        }

        return user;
    }
}