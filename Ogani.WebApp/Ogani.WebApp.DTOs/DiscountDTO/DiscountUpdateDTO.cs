using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.DiscountDTO
{
    public class DiscountUpdateDTO : BaseDTO<int>
    {
        public string Code { get; init; } = null!;
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
