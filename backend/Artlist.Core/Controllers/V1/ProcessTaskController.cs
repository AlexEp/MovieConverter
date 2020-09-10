using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Artlist.Common.Interfaces;
using Artlist.Common.Interfaces.Repository;
using Artlist.Common.Models;
using Artlist.Common.Models.ProcessTasks;
using Artlist.Core.Models;
using Artlist.Core.Models.ProcessTasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace Artlist.Core.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class ProcessTaskController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IArtlistEngine _artlistEngine;
        private readonly ITaskEngine _taskEngine;

        public ProcessTaskController(AppSettings appSettings,
            IArtlistEngine artlistEngine, ITaskEngine taskEngine)
        {
            _appSettings = appSettings;
            _artlistEngine = artlistEngine;
            _taskEngine = taskEngine;
        }
      

        [HttpPost("thumbnails")]
        public void Post([FromBody]ProcessRequestThumbnails processTask)
        {
            var task  = new ProcessExecutorRequest<ProcessRequestThumbnails>(processTask, (p) => { _artlistEngine.ProcessRequestThumbnails(p);});
            _taskEngine.AddTask(task);
        }

        [HttpPost("convert")]
        public  void Post([FromBody]ProcessRequestConvert processTask)
        {
            var task = new ProcessExecutorRequest<ProcessRequestConvert>(processTask, (p) => { _artlistEngine.ProcessRequestConvert(p);});
            _taskEngine.AddTask(task);
        }


    }
}
