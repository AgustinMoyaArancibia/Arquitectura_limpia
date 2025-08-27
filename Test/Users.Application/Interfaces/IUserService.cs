using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Interfaces;

using Users.Application.DTOs;
using static Users.Application.DTOs.UserDto;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync(CancellationToken ct = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(CreateUserRequest req, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateUserRequest req, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

