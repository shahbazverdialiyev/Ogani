using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.ContactDTO
{
    public class ContactDetailReadDTO : ContactReadDTO
    {
        public DateTime CreatedDate { get; init; }
    }
}
