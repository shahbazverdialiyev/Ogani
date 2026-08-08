using Ogani.WebApp.DTOs.DiscountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IDiscountService : IService<DiscountReadDTO, DiscountDetailReadDTO, DiscountCreateDTO, DiscountUpdateDTO>
    {
    }
}
