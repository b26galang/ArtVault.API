using ArtVault.API.Data;
using ArtVault.API.DTOs;
using ArtVault.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtVault.API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        public CommentController(ApplicationDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _dbContext.Comments.ToListAsync();
            var commentsDtos = _mapper.Map<List<CommentDto>>(comments);
            return Ok(commentsDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById([FromRoute] Guid id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = _mapper.Map<CommentDto>(comment);
            return Ok(commentDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = _mapper.Map<Comment>(createCommentDto);

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            var commentDto = _mapper.Map<CommentDto>(comment);

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, commentDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            var comment = await _dbContext.FindAsync<Comment>(id);

            if (comment == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCommentDto, comment);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            _dbContext.Comments.Remove(comment);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
