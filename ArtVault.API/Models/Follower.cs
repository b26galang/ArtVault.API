using System.ComponentModel.DataAnnotations;

namespace ArtVault.API.Models
{
    public class Follower
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid FollowerUserId { get; set; }

        [Required]
        public Guid FollowedUserId { get; set; }

        [Required]
        public DateTime FollowedOn { get; set; } = DateTime.UtcNow;

        public virtual User FollowerUser { get; set; }

        public virtual User FollowedUser { get; set; }

    }
}
