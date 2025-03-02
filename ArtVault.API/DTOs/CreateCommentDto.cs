using System.ComponentModel.DataAnnotations;

namespace ArtVault.API.DTOs
{
    public class CreateCommentDto
    {
        [Required(ErrorMessage = "Comment cannot be empty.")]
        [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
        public string Content { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public Guid PostId { get; set; }
    }
}
