using ArtVault.API.Data;
using ArtVault.API.DTOs;
using ArtVault.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtVault.API.Controllers
{
    [Route("api/users")]
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDto userCreationDto)
        {
            if (userCreationDto == null || string.IsNullOrEmpty(userCreationDto.Auth0UserId))
            {
                return BadRequest("Invalid user data.");
            }

            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Auth0UserId == userCreationDto.Auth0UserId);

            if (existingUser != null)
            {
                return Ok("User already exists.");
            }

            var newUser = _mapper.Map<User>(userCreationDto);

            // Set additional properties if needed
            newUser.CreatedOn = DateTime.UtcNow; // Set current time
            newUser.UserId = Guid.NewGuid();    // Ensure GUID is generated if not automatically

            // Save the user
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, newUser);
        }

        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            var user = await _dbContext.FindAsync<User>(userId);

            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userUpdateDto, user);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

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
