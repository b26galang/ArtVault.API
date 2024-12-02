using ArtVault.API.Data;
using ArtVault.API.DTOs;
using ArtVault.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtVault.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;

        public UserController(ApplicationDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(userRequestDto);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserRequestDto userRequestDto)
        {
            var user = await _dbContext.FindAsync<User>(id);

            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userRequestDto, user);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }


}
