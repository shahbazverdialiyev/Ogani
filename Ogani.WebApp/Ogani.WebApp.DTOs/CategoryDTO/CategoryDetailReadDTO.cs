using Ogani.WebApp.DTOs.Base;
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
        public IReadOnlyCollection<string> ProductNames { get; init; } = [];

        public DateTime CreatedDate { get; init; }
    }
}
