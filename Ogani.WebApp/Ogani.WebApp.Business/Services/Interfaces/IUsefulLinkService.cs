using Ogani.WebApp.DTOs.Client.UsefulLinkDTO;
using Ogani.WebApp.DTOs.UsefulLinkDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IUsefulLinkService : IService<UsefulLinkReadDTO, UsefulLinkReadDTO, UsefulLinkCreateDTO, UsefulLinkUpdateDTO>
    {
        Task<IReadOnlyCollection<UsefulLinkDetailDTO>> GetUsefulLinksForUIAsync();
    }
}
