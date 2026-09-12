using Ogani.WebApp.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Client.UsefulLinkDTO
{
    public class UsefulLinkDetailDTO
    {
        public UsefulLinkSection Section { get; init; }
        public string Name { get; init; } = null!;
        public string Url { get; init; } = null!;
    }
}
