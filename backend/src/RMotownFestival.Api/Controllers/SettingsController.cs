
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using RMotownFestival.Api.Options;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly AppSettingsOptions _options;
        public SettingsController(IOptions<AppSettingsOptions> options)
        {
            _options = options.Value;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AppSettingsOptions))]
        public IActionResult Get()
        {
            return Ok(_options);
        }

        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return Ok("if you see this it works :p");
        }
    }
}
