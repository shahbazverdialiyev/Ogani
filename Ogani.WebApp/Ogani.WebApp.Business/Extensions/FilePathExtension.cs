using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Extensions
{
    public static class FilePathExtension
    {
        public static string ToPhysicalPath(this string relativeUrl, string webRootPath)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return string.Empty;

            string relativePath = relativeUrl.TrimStart('/')
                                             .Replace('/', Path.DirectorySeparatorChar);

            return Path.Combine(webRootPath, relativePath);
        }
    }
}
