namespace Ogani.WebApp.DTOs.Auth
{
    public class AppUserDTO
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public bool IsEmailConfirmed {  get; set; }

        public List<string> Roles { get; set; } = [];
    }
}
