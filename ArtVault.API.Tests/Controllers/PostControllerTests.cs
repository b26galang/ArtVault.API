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
    public class PostControllerTests
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mockMapper;
        private readonly PostController _controller;
        public PostControllerTests()
        {
            _dbContext = GetInMemoryDbContext();
            _mockMapper = GetMockMapper();
            _controller = new PostController(_dbContext, _mockMapper);
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
        public async Task GetPosts_ReturnsPostsWhenExists()
        {
            if (!_dbContext.Posts.Any())
            {
                var post1 = new Post
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "post1.img",
                    Title = "Post 1",
                    Username = "User1"
                };

                var post2 = new Post
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "post2.img",
                    Title = "Post 2",
                    Username = "User2"
                };

                _dbContext.Posts.Add(post1);
                _dbContext.Posts.Add(post2);
                _dbContext.SaveChanges();
            }

            var result = await _controller.GetPosts();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var postDtos = Assert.IsType<List<PostDto>>(okResult.Value);

            Assert.Equal(2, postDtos.Count);
        }

        [Fact]
        public async Task GetPosts_ReturnsEmptyWhenDoesNotExist()
        {
            _dbContext.Posts.RemoveRange(_dbContext.Posts);
            _dbContext.SaveChanges();

            var result = await _controller.GetPosts();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var postDtos = Assert.IsType<List<PostDto>>(okResult.Value);

            Assert.Empty(postDtos);
        }

        [Fact]
        public async Task GetPost_ReturnsWhenPostExists()
        {
            var post3 = new Post
            {
                Id = Guid.NewGuid(),
                ImageUrl = "post3.img",
                Title = "Post 3",
                Username = "User3"
            };

            _dbContext.Posts.Add(post3);
            _dbContext.SaveChanges();

            var result = await _controller.GetPost(post3.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var postDto = Assert.IsType<PostDto>(okResult.Value);

            Assert.Equal(post3.Id, postDto.Id);
            Assert.Equal(post3.ImageUrl, postDto.ImageUrl);
            Assert.Equal(post3.Title, postDto.Title);
        }

        [Fact]
        public async Task GetPost_ReturnsNotFoundsWhenDoesNotExist()
        {
            var nonExistingPostId = Guid.NewGuid();

            var result = await _controller.GetPost(nonExistingPostId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreatePost_CreatesPostWhenModelIsValid()
        {
            var createPostDto = new CreatePostDto
            {
                ImageUrl = "post4.img",
                Title = "Post 4",
                Caption = "Caption for Post 4",
                UserId = Guid.NewGuid(),
                Username = "UserFromPostController"
            };

            var result = await _controller.CreatePost(createPostDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<PostDto>(createdAtActionResult.Value);
            Assert.Equal(createPostDto.ImageUrl, returnValue.ImageUrl);
            Assert.Equal(createPostDto.Title, returnValue.Title);
            Assert.Equal(createPostDto.Caption, returnValue.Caption);
            Assert.Equal(createPostDto.UserId, returnValue.UserId);
            Assert.Equal(createPostDto.Username, returnValue.Username);
        }

        [Fact]
        public async Task UpdatePost_UpdatesPostWhenModelIsValid()
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                ImageUrl = "post2.img",
                Title = "Post 2",
                Username = "User2"
            };
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            var updatePostDto = new UpdatePostDto
            {
                ImageUrl = "updatedpost.img",
                Title = "Updated post",
                Caption = "This didn't have a caption before!"
            };

            var result = await _controller.UpdatePost(post.Id, updatePostDto);

            Assert.IsType<NoContentResult>(result);

            var updatedPost = await _dbContext.Posts.FindAsync(post.Id);
            Assert.Equal("updatedpost.img", updatedPost?.ImageUrl);
            Assert.Equal("Updated post", updatedPost?.Title);
            Assert.Equal("This didn't have a caption before!", updatedPost?.Caption);
        }

        [Fact]
        public async Task DeletePost_DeletesPostWhenPostExists()
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                ImageUrl = "deletesoon.img",
                Title = "Post about to be deleted",
                Username = "newUser"
            };
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            var result = await _controller.DeletePost(post.Id);

            Assert.IsType<NoContentResult>(result);

            var deletedPost = await _dbContext.Posts.FindAsync(post.Id);
            Assert.Null(deletedPost);
        }

        [Fact]
        public async Task DeletePost_ReturnsErrorWhenPostDoesNotExist()
        {
            var nonExistentPostId = Guid.NewGuid();
            var result = await _controller.DeletePost(nonExistentPostId);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
