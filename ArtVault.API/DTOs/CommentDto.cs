namespace ArtVault.API.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
