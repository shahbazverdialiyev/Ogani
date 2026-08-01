using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.SocialLinkDTO
{
    public class SocialLinkCreateDTO
    {
        public string Platform { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
