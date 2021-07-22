using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Rhino.Local.Extensions;

using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhino.Local.Controllers
{
    [ApiController]
    [Route("api/v3")]
    public class BridgeController : ControllerBase
    {
        // members: injection
        private readonly ILogger<BridgeController> logger;
        private readonly HttpClient client;
        
        /// <summary>
        /// Creates a new instance of the controller
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> implementation fot the controller.</param>
        public BridgeController(HttpClient client, ILogger<BridgeController> logger)
        {
            this.logger = logger;
            this.client = client;
        }

        [HttpGet, Route("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        #region *** GET    ***
        [HttpGet]
        [Route("meta/plugins")]
        [Route("meta/assertions")]
        [Route("meta/connectors")]
        [Route("meta/drivers")]
        [Route("meta/locators")]
        [Route("meta/macros")]
        [Route("meta/operators")]
        [Route("meta/reporters")]
        [Route("meta/annotations")]
        [Route("meta/models")]
        [Route("meta/verbs")]
        [Route("meta/attributes")]
        public Task<IActionResult> Get()
        {
            return InvokeGet(route: "All");
        }

        [HttpGet]
        [Route("meta/plugins/{route}")]
        [Route("meta/assertions/{route}")]
        [Route("meta/connectors/{route}")]
        [Route("meta/drivers/{route}")]
        [Route("meta/locators/{route}")]
        [Route("meta/macros/{route}")]
        [Route("meta/operators/{route}")]
        [Route("meta/reporters/{route}")]
        [Route("meta/annotations/{route}")]
        [Route("meta/models/{route}")]
        public Task<IActionResult> Get(string route)
        {
            return InvokeGet(route);
        }

        private async Task<IActionResult> InvokeGet(string route)
        {
            // bridge
            var response = await client.GetAsync(Request.Path.ToString());
            logger.LogInformation("Resolve-RhinoRoute" +
                " -Method GET" +
                $"-Mode Bridge " +
                $"-Route {Request.Path} " +
                $"-Parameter {route} = {response.StatusCode}");

            // get
            return response.GetContentResult();
        }
        #endregion

        #region *** POST   ***
        [HttpPost]
        [Route("rhino/configurations/invoke")]
        [Route("plugins")]
        [Route("models")]
        [Route("integration/create")]
        public Task<IActionResult> Post()
        {
            return InvokePost();
        }

        private async Task<IActionResult> InvokePost()
        {
            // setup
            var reader = new StreamReader(Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var route = Request.Path.ToString();

            // content
            var content = new StringContent(requestBody, Encoding.UTF8, Request.ContentType);

            // bridge
            var response = await client.PostAsync(route, content);
            logger.LogInformation("Resolve-RhinoRoute" +
                " -Method POST " +
                $"-Mode Bridge " +
                $"-Route {Request.Path} " +
                $"-Parameter {route} = {response.StatusCode}");

            // get
            return response.GetContentResult();
        }
        #endregion

        #region *** DELETE ***
        [HttpDelete]
        [Route("models")]
        public Task<IActionResult> Delete()
        {
            return InvokeDelete(route: "All");
        }

        [HttpDelete]
        [Route("models/{route}")]
        public Task<IActionResult> Delete(string route)
        {
            return InvokeDelete(route);
        }

        private async Task<IActionResult> InvokeDelete(string route)
        {
            // bridge
            var response = await client.DeleteAsync(Request.Path.ToString());
            logger.LogInformation("Resolve-RhinoRoute" +
                " -Method DELETE" +
                $"-Mode Bridge " +
                $"-Route {Request.Path} " +
                $"-Parameter {route} = {response.StatusCode}");

            // get
            return response.GetContentResult();
        }
        #endregion
    }
}
