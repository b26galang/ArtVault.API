namespace ArtVault.API.DTOs
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public Guid PostId { get; set; }
    }
}
