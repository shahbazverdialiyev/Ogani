using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Client.ContactDTO
{
    public class ContactDetailDTO
    {
        public string Title { get; init; } = null!;
        public string Content { get; init; } = null!;
        public string? Icon { get; init; }
    }
}
