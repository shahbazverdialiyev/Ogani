using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.UI.Models.Error;

namespace Ogani.WebApp.Controllers
{
    public class ErrorsController : Controller
    {
        private readonly ILogger<ErrorsController> _logger;

        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }

        [Route("Errors/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            bool isAdminArea = statusCodeResult?.OriginalPath?.StartsWith("/admin", StringComparison.OrdinalIgnoreCase) ?? false;
            ViewData["Layout"] = isAdminArea ? "~/Areas/Admin/Views/Shared/_Layout.cshtml" : "~/Views/Shared/_Layout.cshtml";

            var model = new ErrorViewModel
            {
                StatusCode = statusCode,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            switch (statusCode)
            {
                case 404:
                    _logger.LogWarning("404 Page Not Found. Original Path: {Path}", statusCodeResult?.OriginalPath);
                    model.Message = "The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.";
                    return View("NotFound", model);

                case 403:
                    _logger.LogWarning("403 Access Denied. Original Path: {Path}", statusCodeResult?.OriginalPath);
                    model.Message = "You do not have permission to access this resource.";
                    return View("AccessDenied", model);

                default:
                    _logger.LogError("HTTP Status Code Error: {StatusCode} on Path: {Path}", statusCode, statusCodeResult?.OriginalPath);
                    model.Message = "An unexpected error occurred while processing your request.";
                    return View("Error", model);
            }
        }

        [Route("Errors/Unhandled")]
        public IActionResult UnhandledError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error, "Unhandled exception occurred on path: {Path}", exceptionFeature.Path);
            }

            bool isAdminArea = exceptionFeature?.Path?.StartsWith("/admin", StringComparison.OrdinalIgnoreCase) ?? false;
            ViewData["Layout"] = isAdminArea ? "~/Areas/Admin/Views/Shared/_Layout.cshtml" : "~/Views/Shared/_Layout.cshtml";

            var model = new ErrorViewModel
            {
                StatusCode = 500,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = "An internal server error occurred. Our technical team has been notified."
            };

            return View("Error", model);
        }
    }
}