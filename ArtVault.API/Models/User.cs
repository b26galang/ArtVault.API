using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ArtVault.API.Models
{
    public class User
    {
        [Key]
        public required Guid UserId { get; set; }
        public string Auth0UserId { get; set; } // From Auth0        
        public required string Username { get; set; } // Initially set from Auth0

        [Required]
        public string Email { get; set; } // from Auth0

        public string? ProfileImgUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public bool IsAdmin { get; set; }
    }
}
