using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artlist.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Artlist.Core.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ThumbnailsController : ControllerBase
    {
        private readonly IArtlistEngine _artlistEngine;

        public ThumbnailsController(IArtlistEngine artlistEngine)
        {
            _artlistEngine = artlistEngine;
        }
        [HttpGet]
        [Route("file/{id}")]
        public async Task<IActionResult> Get([FromRoute]string id) {

            var stream = await _artlistEngine.GetThumbnails(id);
            return File(stream, "image/png");
        }
    }
}