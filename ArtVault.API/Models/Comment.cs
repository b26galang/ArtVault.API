using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtVault.API.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Comment cannot be empty.")]
        [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual User User { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Post")]
        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }  

    }
}
