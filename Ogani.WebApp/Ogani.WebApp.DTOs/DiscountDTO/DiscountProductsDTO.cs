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
        public int DiscountId { get; init; }

        public IReadOnlyCollection<ProductReadDTO> Products { get; init; } = [];

        public ICollection<int> SelectedProductIds { get; init; } = [];
    }
}
