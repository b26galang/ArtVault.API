using System.ComponentModel.DataAnnotations;

namespace ArtVault.API.DTOs
{
    public class CreatePostDto
    {
        [Required]
        public string ImageUrl { get; set; }
        [StringLength(100, ErrorMessage = "Caption cannot exceed 100 characters.")]
        public string? Title { get; set; }
        [StringLength(250, ErrorMessage = "Caption cannot exceed 250 characters.")]
        public string? Caption { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
