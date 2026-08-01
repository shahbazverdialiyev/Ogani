using Ogani.WebApp.DTOs.ContactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IContactService:IService<ContactReadDTO,ContactCreateDTO,ContactUpdateDTO>
    {
    }
}
