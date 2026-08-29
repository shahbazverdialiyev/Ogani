using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.SocialLinkDTO
{
    public class SocialLinkReadDTO:BaseDTO<int>
    {
        public string Platform { get; init; } = null!;
        public string Url { get; init; } = null!;

        public DateTime CreatedDate { get; init; }
    }
}
