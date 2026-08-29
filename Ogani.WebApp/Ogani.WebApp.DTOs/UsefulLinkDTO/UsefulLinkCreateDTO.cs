using Ogani.WebApp.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.UsefulLinkDTO
{
    public class UsefulLinkCreateDTO
    {
        public UsefulLinkSection Section { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
