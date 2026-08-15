using Ogani.WebApp.DTOs.Base;
using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.CategoryDTO
{
    public class CategoryDetailReadDTO : CategoryReadDTO
    {
        public DateTime CreatedDate { get; init; }
    }
}
