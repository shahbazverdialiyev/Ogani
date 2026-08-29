using Ogani.WebApp.DTOs.UsefulLinkDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IUsefulLinkService : IService<UsefulLinkReadDTO, UsefulLinkReadDTO, UsefulLinkCreateDTO, UsefulLinkUpdateDTO>
    { }
}
