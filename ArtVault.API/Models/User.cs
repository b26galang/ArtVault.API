using System.ComponentModel.DataAnnotations;

namespace ArtVault.API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? ProfileImgUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        public virtual ICollection<Follower> Followers { get; set; } = new List<Follower>();

        public virtual ICollection<Follower> FollowedUsers { get; set; } = new List<Follower>();
    }
}
