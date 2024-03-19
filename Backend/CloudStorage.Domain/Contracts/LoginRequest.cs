using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Domain.Contracts;

public record LoginRequest(
    [Required] [EmailAddress] string Email,
    [Required] string Password);