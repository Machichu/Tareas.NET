namespace SignalRLoginDemo.Models;

public class PendingVerification
{
    public string Token { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool Verified { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
