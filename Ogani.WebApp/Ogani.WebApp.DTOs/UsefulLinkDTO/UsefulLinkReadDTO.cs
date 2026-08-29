using Ogani.WebApp.DTOs.Base;
using Ogani.WebApp.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.UsefulLinkDTO
{
    public class UsefulLinkReadDTO : BaseDTO<int>
    {
        public UsefulLinkSection Section { get; init; }
        public string Name { get; init; } = null!;
        public string Url { get; init; } = null!;

        public DateTime CreatedDate { get; init; }
    }
}
