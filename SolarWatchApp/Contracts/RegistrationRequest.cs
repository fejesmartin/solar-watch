using System.ComponentModel.DataAnnotations;

namespace SolarWatchApp.Contracts;

public record RegistrationRequest(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password);