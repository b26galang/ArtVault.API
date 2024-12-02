namespace ArtVault.API.DTOs
{
    public class CreatePostDto
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public Guid UserId { get; set; }
    }
}
