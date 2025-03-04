using System.ComponentModel.DataAnnotations;

namespace ArtVault.API.DTOs
{
    public class UpdateCommentDto
    {
        [Required]
        public string Content { get; set; }
    }
}
