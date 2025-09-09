using Microsoft.AspNetCore.SignalR;

namespace SignalRLoginDemo.Hubs;

public class VerificationHub : Hub
{
    // El cliente “se suscribe” al grupo identificado por el token de verificación
    public Task JoinVerification(string token)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, token);
    }
}
