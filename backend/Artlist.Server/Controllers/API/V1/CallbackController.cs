using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artlist.Common.Models;
using Artlist.Common.Models.DTO;
using Artlist.Common.Models.ProcessTasks;
using Artlist.Core.Models;
using Artlist.Server.Models.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Artlist.Server.Controllers.API.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly ILogger<CallbackController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IHubContext<AppHub> _apphub;

        public CallbackController(ILogger<CallbackController> logger, IHubContext<AppHub> apphub)
        {
            _logger = logger;
            _apphub = apphub;
        }


        [HttpPost("thumbnails")]
        public async Task ThumbnailsFile(ProcessThumbnailsResponse response) {

            var proccesEvent = new ProccesEvent() {
                Type = ProcessRequestType.CreateThumbnails,
                FileId = response.UploadedFileId,
                Percent = 0,
                Data = response
            };

            proccesEvent.Status = response.Status;
            switch (response.Status)
            {
                case ProcessStatusType.Started:
                    proccesEvent.Percent = 0;
                    break;
                case ProcessStatusType.Completed:
                    proccesEvent.Percent = 100;
                    break;
                case ProcessStatusType.InProcess:
                    break;
                case ProcessStatusType.Failed:
                    proccesEvent.Massege = response.ErrorMassege;
                    break;
                default:
                    break;
            }

            await _apphub.Clients.All.SendAsync("procces_event", proccesEvent);
        }

        [HttpPost("fileconvert")]
        public async Task ConvertFile(ProcessConvertResponse response)
        {
            var proccesEvent = new ProccesEvent() { Type = ProcessRequestType.ConvertFile, 
                FileId = response.UploadedFileId,
                Percent = 0 ,
                Data = response
            };
            proccesEvent.Status = response.Status;
            switch (response.Status)
            {
                case ProcessStatusType.Started:
                    proccesEvent.Percent = 0;
                    break;
                case ProcessStatusType.Completed:
                    proccesEvent.Percent = 100;
                    break;
                case ProcessStatusType.InProcess:
                    break;
                case ProcessStatusType.Failed:
                    proccesEvent.Massege = response.ErrorMassege;
                    break;
                default:
                    break;
            }

            await _apphub.Clients.All.SendAsync("procces_event", proccesEvent);
        }
    }
}