using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace SettingsProxyAPI.Controllers
{
    [ApiController,
    OpenApiTag(nameof(SettingsController), Description = "Add settings"),
    Route("api/auth")]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }
    }
}
