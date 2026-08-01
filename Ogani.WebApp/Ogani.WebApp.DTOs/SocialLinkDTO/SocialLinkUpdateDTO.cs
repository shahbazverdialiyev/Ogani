using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.SocialLinkDTO
{
    public class SocialLinkUpdateDTO:BaseDTO<int>
    {
        public string Platform { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
