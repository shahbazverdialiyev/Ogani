namespace Ogani.WebApp.DTOs.Auth
{
    public record ResetPasswordDTO(string id, string token)
    {
        public string Id { get; init; } = id;
        public string Token { get; init; } = token;
        public string NewPassword { get; init; } = null!;
    }
}
