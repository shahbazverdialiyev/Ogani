using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.ContactDTO
{
    public class ContactReadDTO:BaseDTO<int>
    {
        public string Title { get; init; } = null!;
        public string Content { get; init; } = null!;

        public DateTime CreatedDate { get; init; }
    }
}
