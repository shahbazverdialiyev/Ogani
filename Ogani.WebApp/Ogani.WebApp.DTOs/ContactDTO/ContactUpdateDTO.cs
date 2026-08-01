using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.ContactDTO
{
    public class ContactUpdateDTO:BaseDTO<int>
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
