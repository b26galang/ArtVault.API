namespace ArtVault.API.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Auth0UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileImgUrl { get; set; }
        public bool IsAdmin { get; set; }
    }
}
