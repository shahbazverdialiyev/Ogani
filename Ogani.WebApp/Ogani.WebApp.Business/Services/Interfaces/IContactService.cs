using Ogani.WebApp.DTOs.ContactDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IContactService:IService<ContactReadDTO,ContactDetailReadDTO,ContactCreateDTO,ContactUpdateDTO>
    {
    }
}
