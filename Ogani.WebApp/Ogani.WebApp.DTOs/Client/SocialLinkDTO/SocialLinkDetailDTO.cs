using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Client.SocialLinkDTO
{
    public class SocialLinkDetailDTO
    {
        public string Platform { get; init; } = null!;
        public string Url { get; init; } = null!;
    }
}
