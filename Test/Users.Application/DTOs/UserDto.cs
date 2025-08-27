namespace Users.Application.DTOs;

public record UserDto(int Id, string Username, string Email, string Role, bool IsActive);

public record CreateUserRequest(string Username, string Email, string Password);

public record UpdateUserRequest(string Username, string Email, string? Password, string Role, bool IsActive);
