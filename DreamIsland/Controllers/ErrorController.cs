namespace DreamIsland.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;

    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 400:
                    ViewBag.ErrorMessage = "Bad request";
                    logger.LogWarning($"401 error occured. Path = " +
                    $"{statusCodeResult.OriginalPath} and QueryString = " +
                    $"{statusCodeResult.OriginalQueryString}");
                    break;
                case 401:
                    ViewBag.ErrorMessage = "Authorization required.";
                    logger.LogWarning($"401 error occured. Path = " +
                    $"{statusCodeResult.OriginalPath} and QueryString = " +
                    $"{statusCodeResult.OriginalQueryString}");
                    break;
                case 404:
                    ViewBag.ErrorMessage = "Oops! This page could not be found.";
                    logger.LogWarning($"404 error occured. Path = " +
                    $"{statusCodeResult.OriginalPath} and QueryString = " +
                    $"{statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View("ClientError");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            logger.LogError($"The path {exceptionDetails.Path} " +
            $"threw an exception {exceptionDetails.Error}");

            return View("Error");
        }
    }
}
