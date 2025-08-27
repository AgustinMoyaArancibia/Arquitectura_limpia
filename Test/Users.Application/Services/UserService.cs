using System.Security.Cryptography;
using System.Text;
using Users.Application.DTOs;
using Users.Application.Interfaces;
using Users.Domain.Entities;
using static Users.Application.DTOs.UserDto;

namespace Users.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken ct = default)
        {
            var users = await _repo.GetAllAsync(ct);
            return users.Select(u => new UserDto(u.Id, u.Username, u.Email, u.Role, u.IsActive)).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            return u is null ? null : new UserDto(u.Id, u.Username, u.Email, u.Role, u.IsActive);
        }

        public async Task<int> CreateAsync(CreateUserRequest req, CancellationToken ct = default)
        {
            // Validaciones de unicidad
            if (await _repo.UsernameExistsAsync(req.Username, null, ct))
                throw new InvalidOperationException("Username already exists.");
            if (await _repo.EmailExistsAsync(req.Email, null, ct))
                throw new InvalidOperationException("Email already exists.");

            // Mapear DTO -> Entidad
            var entity = new User
            {
                Username = req.Username,
                Email = req.Email,
                PasswordHash = Hash(req.Password),
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repo.AddAsync(entity, ct);
            return created.Id;
        }

        public async Task UpdateAsync(int id, UpdateUserRequest req, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("User not found.");

            if (await _repo.UsernameExistsAsync(req.Username, id, ct))
                throw new InvalidOperationException("Username already exists.");
            if (await _repo.EmailExistsAsync(req.Email, id, ct))
                throw new InvalidOperationException("Email already exists.");

            u.Username = req.Username;
            u.Email = req.Email;
            u.Role = req.Role;
            u.IsActive = req.IsActive;

            if (!string.IsNullOrWhiteSpace(req.Password))
                u.PasswordHash = Hash(req.Password);

            await _repo.UpdateAsync(u, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("User not found.");
            await _repo.DeleteAsync(u, ct);
        }

        // Hash simple para demo (en real: BCrypt/Argon2)
        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}
