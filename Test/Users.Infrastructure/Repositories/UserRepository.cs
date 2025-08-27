using Microsoft.EntityFrameworkCore;
using Users.Application.Interfaces;
using Users.Domain.Entities;
using Users.Infrastructure.Data;

namespace Users.Infrastructure.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
        => db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<List<User>> GetAllAsync(CancellationToken ct = default)
        => db.Users.AsNoTracking().OrderBy(u => u.Id).ToListAsync(ct);

    public async Task<User> AddAsync(User e, CancellationToken ct = default)
    { db.Users.Add(e); await db.SaveChangesAsync(ct); return e; }

    public async Task UpdateAsync(User e, CancellationToken ct = default)
    { db.Users.Update(e); await db.SaveChangesAsync(ct); }

    public async Task DeleteAsync(User e, CancellationToken ct = default)
    { db.Users.Remove(e); await db.SaveChangesAsync(ct); }

    public Task<bool> UsernameExistsAsync(string username, int? excludeId = null, CancellationToken ct = default)
        => db.Users.AnyAsync(u => u.Username == username && (!excludeId.HasValue || u.Id != excludeId), ct);

    public Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken ct = default)
        => db.Users.AnyAsync(u => u.Email == email && (!excludeId.HasValue || u.Id != excludeId), ct);
}
