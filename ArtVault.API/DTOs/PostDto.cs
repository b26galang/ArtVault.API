namespace ArtVault.API.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int LikeCount { get; set; }
    }
}
