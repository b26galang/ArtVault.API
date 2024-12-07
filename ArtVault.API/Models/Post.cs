using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtVault.API.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string ImageUrl { get; set; }

        [StringLength(100, ErrorMessage = "Caption cannot exceed 100 characters.")]
        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Caption cannot exceed 250 characters.")]
        public string? Caption { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public int LikeCount { get; set; } = 0;

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
