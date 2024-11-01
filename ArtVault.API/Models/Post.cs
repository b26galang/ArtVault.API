﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtVault.API.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        public ICollection<string> ImageUrls { get; set; } = new List<string>();

        [StringLength(100, ErrorMessage = "Caption cannot exceed 100 chararacters.")]
        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Caption cannot exceed 250 chararacters.")]
        public string? Caption { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public int LikeCount { get; set; } = 0;

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
