using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtVault.API.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        public ICollection<string> ImageUrls { get; set; } = new List<string>();
        
        public string? Title { get; set; }

        public string? Caption { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

        public int LikeCount { get; set; } = 0;

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
