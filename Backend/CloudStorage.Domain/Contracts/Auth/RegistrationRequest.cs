using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Domain.Contracts.Auth;

public record RegistrationRequest(
    [Required] string Username,
    [Required] [EmailAddress] string Email,
    [Required] string Password);