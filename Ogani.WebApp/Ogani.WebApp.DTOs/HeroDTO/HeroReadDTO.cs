using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.HeroDTO
{
    public class HeroReadDTO : BaseDTO<int>
    {
        public string Title { get; init; } = null!;
        public string Subtitle { get; init; } = null!;
        public string? Description { get; init; }
        public string ButtonText { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public bool IsActive { get; init; }
    }
}
