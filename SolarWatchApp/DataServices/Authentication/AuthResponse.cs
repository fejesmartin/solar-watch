namespace SolarWatchApp.DataServices.Authentication;

public record AuthResponse(string Email, string UserName, string Token);