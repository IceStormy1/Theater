using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Caches;

namespace Theater.Core.Caches;

public sealed class ConnectionsCache : IConnectionsCache
{
    private const string PrefixKey = "rooms_";

    private readonly IRedisClient _redis;

    public ConnectionsCache(IRedisClient redis) 
        => _redis = redis;

    public async Task SetConnection(Guid userId, string connectionId)
    {
        var connections = await GetConnections(userId);
        if (connections.Add(connectionId))
        {
            await _redis.Db0.AddAsync(
                key: GetChatConnectionsCacheKey(userId),
                value: connections);
        }
    }

    public async Task<HashSet<string>> GetConnections(Guid userId)
    {
        var connections = await _redis.Db0.GetAsync<HashSet<string>>(
            key: GetChatConnectionsCacheKey(userId));

        return connections ?? new HashSet<string>();
    }

    public async Task RemoveConnection(Guid userId, string connectionId)
    {
        var connections = await GetConnections(userId);
        if (connections.Remove(connectionId))
        {
            var key = GetChatConnectionsCacheKey(userId);
            if (connections.Any())
                await _redis.Db0.AddAsync(key: key, value: connections);
            else
                await _redis.Db0.RemoveAsync(key);
        }
    }

    public async Task<IDictionary<Guid, HashSet<string>>> GetUserConnectionsByIds(IEnumerable<Guid> userIds)
    {
        var dict = await _redis.Db0.GetAllAsync<HashSet<string>>(
            new HashSet<string>(userIds.Select(GetChatConnectionsCacheKey)));

        return dict.ToDictionary(x => GetIdFromKey(x.Key), y => y.Value);
    }

    public async Task ClearConnections(Guid userId)
    {
        var key = GetChatConnectionsCacheKey(userId);
        await _redis.Db0.RemoveAsync(key);
    }

    private static string GetChatConnectionsCacheKey(Guid userId)
    {
        var tt = PrefixKey + userId;
        return PrefixKey + userId;
    }

    private static Guid GetIdFromKey(string key)
    {
        return Guid.Parse(key[PrefixKey.Length..]);
    }
}