using Microsoft.EntityFrameworkCore;
using ArtVault.API.Controllers;
using ArtVault.API.Data;
using ArtVault.API.Models;
using AutoMapper;
using ArtVault.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using ArtVault.API.Profiles;

namespace ArtVault.API.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mockMapper;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _dbContext = GetInMemoryDbContext();
            _mockMapper = GetMockMapper();
            _controller = new UserController(_dbContext, _mockMapper);
        }
        private ApplicationDBContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new ApplicationDBContext(options);
            return dbContext;
        }

        private IMapper GetMockMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return config.CreateMapper();
        }

        [Fact]
        public async Task GetUsers_ReturnsListsOfUsers()
        {
            var user1 = new User
            {
                UserId = Guid.NewGuid(),
                Auth0UserId = "auth0|5f2f8b9b3e9bda001c5a98f2",
                Username = "CommanderShepard",
                Email = "CommanderShepard@test.com",
                ProfileImgUrl = "https://commandershepard.com/image.jpg",
                IsAdmin = true
            };

            var user2 = new User
            {
                UserId = Guid.NewGuid(),
                Auth0UserId = "auth0|5f2f8b9a2e9bda001c5a98f2",
                Username = "Garrus",
                Email = "Garrus@test.com",
                ProfileImgUrl = "https://garrus.com/image.jpg",
                IsAdmin = true
            };

            _dbContext.Users.Add(user1);
            _dbContext.Users.Add(user2);
            _dbContext.SaveChanges();

            var result = await _controller.GetUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
            Assert.Contains(returnedUsers, u => u.Username == "CommanderShepard");
            Assert.Contains(returnedUsers, u => u.Username == "Garrus");
        }

        [Fact]
        public async Task GetUser_ReturnsUserWhenUserExists()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Auth0UserId = "auth0|5f2f8b9a2e9bda001c5a98f2",
                Username = "Liara",
                Email = "Liara@test.com",
                ProfileImgUrl = "https://liara.com/image.jpg",
                IsAdmin = true
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var result = await _controller.GetUser(user.UserId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);

            Assert.Equal(user.UserId, returnedUser.UserId);
            Assert.Equal(user.Username, returnedUser.Username);
            Assert.Equal(user.Email, returnedUser.Email);
            Assert.Equal(user.ProfileImgUrl, returnedUser.ProfileImgUrl);
            Assert.Equal(user.IsAdmin, returnedUser.IsAdmin);
        }

        [Fact]
        public async Task GetUserByAuth0UserId_ReturnsUser_WhenUserExists()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Auth0UserId = "auth0|5f2f8b9a2e9bda001c5a98f2",
                Username = "Thali",
                Email = "Thali@test.com",
                ProfileImgUrl = "https://thane.com/image.jpg",
                IsAdmin = true
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var result = await _controller.GetUserByAuth0UserId(user.Auth0UserId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);

            Assert.Equal(user.UserId, returnedUser.UserId);
            Assert.Equal(user.Username, returnedUser.Username);
            Assert.Equal(user.Email, returnedUser.Email);
            Assert.Equal(user.ProfileImgUrl, returnedUser.ProfileImgUrl);
            Assert.Equal(user.IsAdmin, returnedUser.IsAdmin);

        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedAtAction_WhenUserIsCreated()
        {
            var dbContext = GetInMemoryDbContext();
            var mapper = GetMockMapper();
            var controller = new UserController(dbContext, mapper);

            var newUserDto = new UserCreationDto
            {
                Auth0UserId = "auth0|5f2f8b9b3e9bda123c5a98f2",
                Username = "Aria",
                Email = "Aria@email.com"
            };

            var result = await controller.CreateUser(newUserDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdUser = Assert.IsType<User>(createdResult.Value);

            Assert.Equal("auth0|5f2f8b9b3e9bda123c5a98f2", createdUser.Auth0UserId);
            Assert.Equal("Aria", createdUser.Username);
            Assert.Equal("Aria@email.com", createdUser.Email);
            Assert.False(createdUser.IsAdmin);
        }

        [Fact]
        public async Task UpdateUser_WhenUserExists_ReturnsNoContent()
        {
            var testUser = new User
            {
                UserId = Guid.NewGuid(),
                Auth0UserId = "auth0|5f2f8b9h5e9bda123c5a98f2",
                Username = "Vanguard",
                Email = "vanguard@example.com",
                ProfileImgUrl = "https://vanguard.com/image.jpg",
                IsAdmin = false
            };

            _dbContext.Users.Add(testUser);
            await _dbContext.SaveChangesAsync();

            var updateDto = new UserUpdateDto
            {
                Username = "Adept",
                ProfileImgUrl = "https://adept.com/image.jpg"
            };

            var result = await _controller.UpdateUser(testUser.UserId, updateDto);

            Assert.IsType<NoContentResult>(result);

            var updatedUser = await _dbContext.Users.FindAsync(testUser.UserId);
            Assert.NotNull(updatedUser);
            Assert.Equal("Adept", updatedUser.Username);
            Assert.Equal("https://adept.com/image.jpg", updatedUser.ProfileImgUrl);
        }
    }
}
