using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.HeroDTO
{
    public class HeroCreateDTO
    {
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public string? Description { get; set; }

        public string ButtonText { get; set; } = null!;
        public string ButtonUrl { get; set; } = null!;

        public IFormFile Image { get; set; } = null!;
    }
}
