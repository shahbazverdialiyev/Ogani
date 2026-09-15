using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Auth
{
    public class UpdateUserRolesDTO
    {
        public string UserId { get; set; } = null!;
        public List<string> Roles { get; set; } = [];
    }
}
