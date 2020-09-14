using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Artlist.Common.Interfaces;
using Artlist.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Artlist.Server.Controllers
{
    [Route("[controller]")]
    public class ResourceController : Controller
    {
        private readonly IArtlistEngine _artlistEngine;

        public ResourceController(IArtlistEngine artlistEngine)
        {
            _artlistEngine = artlistEngine;
        }

        [HttpGet("{fileid}")]
        public async Task<IActionResult> Index([FromRoute]string fileid)
        {

            Stream stream = await _artlistEngine.GetThumbnails(fileid);

            return new FileStreamResult(stream, "image/png");
        
        }
    }
}