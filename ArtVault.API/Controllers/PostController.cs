using ArtVault.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtVault.API.DTOs;
using ArtVault.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArtVault.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;

        public PostController(ApplicationDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _dbContext.Posts.ToListAsync();
            var postDtos = _mapper.Map<List<PostDto>>(posts);
            return Ok(postDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost([FromRoute] Guid id)
        {
            var post = await _dbContext.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var postDto = _mapper.Map<PostDto>(post);
            return Ok(postDto);
        }

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetCommentsForPost(Guid postId)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.PostId == postId)
                .ToListAsync();

            if (comments.Count == 0)
            {
                return NotFound("No comments found for this post.");
            }

            var commentDtos = _mapper.Map<List<CommentDto>>(comments);

            return Ok(commentDtos);
        }

        [HttpGet("{userId}/posts")]
        public async Task<IActionResult> GetPostsForUser(Guid userId)
        {
            var posts = await _dbContext.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (posts.Count == 0)
            {
                return NotFound("No posts found for this user.");
            }

            var postDtos = _mapper.Map<List<PostDto>>(posts);

            return Ok(postDtos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = _mapper.Map<Post>(createPostDto);

            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();

            var postDto = _mapper.Map<PostDto>(post);

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, postDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            var post = await _dbContext.FindAsync<Post>(id);

            if (post == null)
            {
                return NotFound();
            }

            _mapper.Map(updatePostDto, post);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var post = await _dbContext.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            _dbContext.Posts.Remove(post);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
