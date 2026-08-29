using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Base
{
    public interface IRemovableImageDTO : IWithImageDTO
    {
        bool RemoveExistingImage { get; set; }
    }
}
