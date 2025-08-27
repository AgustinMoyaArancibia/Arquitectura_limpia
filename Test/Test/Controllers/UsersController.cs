using Microsoft.AspNetCore.Mvc;
using Users.Application.DTOs;
using Users.Application.Interfaces;
using static Users.Application.DTOs.UserDto;

namespace Test.Api.Controllers // <-- CAMBIÁ a Users.Api.Controllers si tu API se llama Users.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll(CancellationToken ct)
        {
            var list = await _service.GetAllAsync(ct);
            return Ok(list);
        }

        // GET: api/Users/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken ct)
        {
            var u = await _service.GetByIdAsync(id, ct);
            return u is null ? NotFound() : Ok(u);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateUserRequest req, CancellationToken ct)
        {
            var id = await _service.CreateAsync(req, ct);
            // Devuelve 201 y la ubicación del recurso creado
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        // PUT: api/Users/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest req, CancellationToken ct)
        {
            await _service.UpdateAsync(id, req, ct);
            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _service.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
