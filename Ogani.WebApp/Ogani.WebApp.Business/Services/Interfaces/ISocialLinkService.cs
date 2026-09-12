using Ogani.WebApp.DTOs.Client.SocialLinkDTO;
using Ogani.WebApp.DTOs.SocialLinkDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface ISocialLinkService : IService<SocialLinkReadDTO, SocialLinkReadDTO, SocialLinkCreateDTO, SocialLinkUpdateDTO>
    {
        Task<IReadOnlyCollection<SocialLinkDetailDTO>> GetSocialLinksForUIAsync();
    }
}
