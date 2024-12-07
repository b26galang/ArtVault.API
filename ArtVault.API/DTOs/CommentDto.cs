namespace ArtVault.API.DTOs
{
    public class CommentDto
    {
        // Readonly
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public Guid PostId { get; set; }
    }
}
