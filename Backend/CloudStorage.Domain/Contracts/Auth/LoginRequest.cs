using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Domain.Contracts.Auth;

public record LoginRequest(
    [Required] [EmailAddress] string Email,
    [Required] string Password);