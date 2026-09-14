using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.DiscountDTO
{
    public class DiscountProductsDTO
    {
        public int DiscountId { get; set; }

        public IReadOnlyCollection<ProductReadDTO> Products { get; set; } = [];

        public ICollection<int> SelectedProductIds { get; set; } = [];
    }
}
