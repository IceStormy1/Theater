using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Theater.Abstractions.Caches;

namespace Theater.Core.Caches;

public sealed class UserAccountCache : IUserAccountCache
{
    private const string PrefixKey = "externalUserId_";

    private readonly IRedisClient _redis;

    public UserAccountCache(IRedisClient redis)
    {
        _redis = redis;
    }

    public async Task LinkUserIds(Guid externalUserId, Guid innerUserId)
    {
        var externalUserIdCacheKey = GetExternalUserIdCacheKey(externalUserId);
        var isLinkExists = await _redis.Db0.ExistsAsync(externalUserIdCacheKey);

        if (!isLinkExists)
        {
            await _redis.Db0.AddAsync(
                key: externalUserIdCacheKey,
                value: innerUserId);
        }
    }

    public Task<Guid?> GetInnerUserIdByExternalId(Guid externalUserId)
        => _redis.Db0.GetAsync<Guid?>(key: GetExternalUserIdCacheKey(externalUserId));
    
    private static string GetExternalUserIdCacheKey(Guid externalUserId)
        => PrefixKey + externalUserId;

    private static Guid GetExternalUserIdFromKey(string key)
        => Guid.Parse(key[PrefixKey.Length..]);
}