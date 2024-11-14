using System.Collections.Concurrent;

namespace Tests;

internal record User(string Role, int UserId);

public class RoleManagerDictionary
{
    private readonly Dictionary<string, User> _record = new();

    public async Task<bool> TryAssign(string role, int userId)
    {
        var user = new User(role, userId);
        await Task.Delay(100);
        return _record.TryAdd(role, user);
    }
}

public class RoleManagerConcurrentDictionary
{
    private readonly ConcurrentDictionary<string, User> _record = new();

    public async Task<bool> TryAssign(string role, int userId)
    {
        var user = new User(role, userId);
        await Task.Delay(100);
        return _record.TryAdd(role, user);
    }
}