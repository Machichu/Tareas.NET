using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRLoginDemo.Hubs;
using SignalRLoginDemo.Services;

namespace SignalRLoginDemo.Controllers;

public class AuthController : Controller
{
    private readonly VerificationStore _store;
    private readonly IHubContext<VerificationHub> _hub;

    public AuthController(VerificationStore store, IHubContext<VerificationHub> hub)
    {
        _store = store;
        _hub = hub;
    }

    [HttpGet]  // GET /Auth/Login
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost] // POST /Auth/Login
    public IActionResult Login(string email, string password)
    {
        // En un caso real validar�as credenciales; ac� simulamos
        var verification = _store.Create(email);

        // Mostramos la URL de verificaci�n que el usuario deber�a abrir
        ViewBag.VerifyUrl = Url.ActionLink(nameof(Verify), values: new { token = verification.Token });
        ViewBag.Token = verification.Token;
        ViewBag.Email = verification.Email;

        return View("AwaitingVerification");
    }

    [HttpGet] // GET /Auth/Verify?token=...
    public async Task<IActionResult> Verify(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return BadRequest("Falta token.");

        var ok = _store.MarkVerified(token);
        if (!ok) return NotFound("Token inv�lido.");

        // Notificar al grupo = token
        await _hub.Clients.Group(token).SendAsync("Verified");

        return Content("�Email verificado! Volv� a la pesta�a del login: se redirigir� sola.");
    }
}
