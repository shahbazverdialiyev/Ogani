using Microsoft.AspNetCore.Http;
using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.HeroDTO
{
    public class HeroUpdateDTO : BaseDTO<int>
    {
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public string? Description { get; set; }

        public string ButtonText { get; set; } = null!;
        public string ButtonUrl { get; set; } = null!;

        public IFormFile? Image { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
