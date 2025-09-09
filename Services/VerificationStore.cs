using System.Collections.Concurrent;
using SignalRLoginDemo.Models;

namespace SignalRLoginDemo.Services;

public class VerificationStore
{
    private readonly ConcurrentDictionary<string, PendingVerification> _byToken = new();

    public PendingVerification Create(string email)
    {
        var token = Guid.NewGuid().ToString("N");
        var item = new PendingVerification { Token = token, Email = email, Verified = false };
        _byToken[token] = item;
        return item;
    }

    public PendingVerification? Get(string token) =>
        _byToken.TryGetValue(token, out var item) ? item : null;

    public bool MarkVerified(string token)
    {
        if (!_byToken.TryGetValue(token, out var item)) return false;
        item.Verified = true;
        return true;
    }
}
