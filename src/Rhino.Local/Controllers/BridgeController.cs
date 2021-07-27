using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Rhino.Local.Extensions;

using System.Net.Http;
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

        #region *** Get    ***
        [HttpGet]
        [Route("configurations")]
        [Route("configurations/{path}")]
        [Route("environment")]
        [Route("environment/{path}")]
        [Route("environment/sync")]
        [Route("logs")]
        [Route("logs/{path}")]
        [Route("logs/{path}/export")]
        [Route("logs/{path}/size/{parameter}")]
        [Route("meta/annotations")]
        [Route("meta/annotations/{path}")]
        [Route("meta/assertions")]
        [Route("meta/assertions/{path}")]
        [Route("meta/connectors")]
        [Route("meta/connectors/{path}")]
        [Route("meta/drivers")]
        [Route("meta/drivers/{path}")]
        [Route("meta/locators")]
        [Route("meta/locators/{path}")]
        [Route("meta/macros")]
        [Route("meta/macros/{path}")]
        [Route("meta/operators")]
        [Route("meta/operators/{path}")]
        [Route("meta/plugins")]
        [Route("meta/plugins/{path}")]
        [Route("meta/reporters")]
        [Route("meta/reporters/{path}")]
        [Route("meta/models")]
        [Route("meta/models/{path}")]
        [Route("meta/verbs")]
        [Route("meta/attributes")]
        [Route("models")]
        [Route("models/{path}")]
        [Route("models/{path}/configurations")]
        [Route("ping/rhino")]
        [Route("plugins")]
        [Route("plugins/{path}")]
        [Route("rhino/async/collections/invoke/{path}")]
        [Route("rhino/async/configurations/invoke/{path}")]
        [Route("rhino/async/configurations/{path}/collections/invoke/{parameter}")]
        [Route("rhino/async/status/{path}")]
        [Route("rhino/async/status")]
        [Route("rhino/collections/invoke/{path}")]
        [Route("rhino/configurations/invoke/{path}")]
        [Route("rhino/configurations/{path}/collections/invoke/{parameter}")]
        [Route("tests")]
        [Route("tests/{path}")]
        [Route("tests/{path}/configurations")]
        public Task<IActionResult> Get()
        {
            return client.InvokeBridgeRequest(Request, Response, logger);
        }
        #endregion

        #region *** Post   ***
        [HttpPost]
        [Route("configurations")]
        [Route("debug")]
        [Route("integration/create")]
        [Route("models")]
        [Route("models/{path}")]
        [Route("plugins")]
        [Route("rhino/async/configurations/invoke")]
        [Route("rhino/async/configurations/{path}/collections/invoke")]
        [Route("rhino/configurations/invoke")]
        [Route("rhino/configurations/{path}/collections/invoke")]
        [Route("tests")]
        [Route("tests/{path}")]
        public Task<IActionResult> Post()
        {
            return client.InvokeBridgeRequest(Request, Response, logger);
        }
        #endregion

        #region *** Delete ***
        [HttpDelete]
        [Route("configurations")]
        [Route("configurations/{path}")]
        [Route("environment")]
        [Route("environment/{path}")]
        [Route("models")]
        [Route("models/{path}")]
        [Route("plugins")]
        [Route("plugins/{path}")]
        [Route("rhino/async/status")]
        [Route("rhino/async/status/{path}")]
        [Route("tests")]
        [Route("tests/{path}")]
        public Task<IActionResult> Delete()
        {
            return client.InvokeBridgeRequest(Request, Response, logger);
        }
        #endregion

        #region *** Put    ***
        [HttpPut]
        [Route("configurations/{path}")]
        [Route("environment/{path}")]
        public Task<IActionResult> Put()
        {
            return client.InvokeBridgeRequest(Request, Response, logger);
        }
        #endregion

        #region *** Patch  ***
        [HttpPatch]
        [Route("models/{path}")]
        [Route("models/{path}/configurations/{parameter}")]
        [Route("tests/{path}")]
        [Route("tests/{path}/configurations/{parameter}")]
        public Task<IActionResult> Patch()
        {
            return client.InvokeBridgeRequest(Request, Response, logger);
        }
        #endregion
    }
}