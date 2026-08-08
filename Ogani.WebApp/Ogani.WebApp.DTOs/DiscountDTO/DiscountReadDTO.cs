using Ogani.WebApp.DTOs.Base;
using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.DiscountDTO
{
    public class DiscountReadDTO:BaseDTO<int>
    {
        public string Code { get; init; } = null!;
        public decimal DiscountPercentage { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
