using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Domain.Contracts;

public record RegistrationRequest(
    [Required] string Username,
    [Required] [EmailAddress] string Email,
    [Required] string Password);