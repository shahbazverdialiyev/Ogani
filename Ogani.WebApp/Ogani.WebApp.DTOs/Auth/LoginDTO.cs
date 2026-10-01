namespace Ogani.WebApp.DTOs.Auth
{
    public record LoginDTO
    {
        public string UserNameOrEmail { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}