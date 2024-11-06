using System.ComponentModel.DataAnnotations;

namespace ArtVault.API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 2 characters long.")]
        [RegularExpression(@"^[a-zA-Z-9]*$", ErrorMessage = "Username can only contain alphanumeric characters.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Email address is invalid.")]
        public string Email { get; set; }

        public string? ProfileImgUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
