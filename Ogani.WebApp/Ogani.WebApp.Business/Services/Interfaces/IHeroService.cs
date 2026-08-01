using Ogani.WebApp.DTOs.HeroDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IHeroService:IService<HeroReadDTO,HeroCreateDTO,HeroUpdateDTO>
    {
    }
}
