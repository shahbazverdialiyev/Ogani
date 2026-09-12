using Ogani.WebApp.DTOs.Client.ContactDTO;
using Ogani.WebApp.DTOs.ContactDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IContactService : IService<ContactReadDTO, ContactDetailReadDTO, ContactCreateDTO, ContactUpdateDTO>
    {
        Task<IReadOnlyCollection<ContactDetailDTO>> GetContactsForUIAsync();
        Task<ContactDetailDTO?> GetPhoneAsync();
        Task<ContactDetailDTO?> GetEmailAsync();
    }
}
