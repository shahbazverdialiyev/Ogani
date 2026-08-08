using Ogani.WebApp.DTOs.SocialLinkDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface ISocialLinkService:IService<SocialLinkReadDTO,SocialLinkReadDTO,SocialLinkCreateDTO,SocialLinkUpdateDTO>
    {
    }
}
