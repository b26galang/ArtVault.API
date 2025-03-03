using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtVault.API.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Content { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }  

    }
}
