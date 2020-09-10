using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Artlist.Common.Interfaces;
using Artlist.Common.Models;
using Artlist.Common.Models.DTO;
using Artlist.Common.Models.ProcessTasks;
using Artlist.Server.Models;
using Artlist.Server.Models.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Artlist.Server.Controllers.API.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IArtlistEngine _artlistEngine;
        private readonly IHubContext<AppHub> _apphub;

        public FileController(ILogger<FileController> logger, 
            AppSettings appSettings,
            IArtlistEngine artlistEngine,
            IHubContext<AppHub> apphub)
        {
            _logger = logger;
            _appSettings = appSettings;
            _artlistEngine = artlistEngine;
            _apphub = apphub;
        }


        [HttpPost,DisableRequestSizeLimit]
        public async Task<IActionResult> PostFile()
        {
            UploadedFile uploadedFile = new UploadedFile();
            try
            {
                if (Request.Form.Files.Count < 1)
                    return BadRequest();

                var file = Request.Form.Files[0];
 
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    //Save the file
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        ms.Position = 0;
                        uploadedFile = await _artlistEngine.SaveFileAsync(ms, fileName);
                    }

                    if (uploadedFile == null)
                    {
                        throw new Exception("Failed to upload file");
                    }

                    await _apphub.Clients.All.SendAsync("procces_event", new ProccesEvent() {
                        Type = ProcessRequestType.FileUpload,
                        Status = ProcessStatusType.Completed,
                        FileId = uploadedFile.Id, 
                        Percent = 100f});

                    Task proccesTask = new Task(() =>  ProccesUploadFile(uploadedFile));
                    proccesTask.Start();
                  

                    return Ok(uploadedFile);
                }
                else
                {
                    throw new Exception("No files were found");
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(uploadedFile.Id))
                {
                    await _apphub.Clients.All.SendAsync("procces_event", new ProccesEvent()
                    {
                        Type = ProcessRequestType.FileUpload,
                        Status = ProcessStatusType.Failed,
                        FileId = "",
                        Percent = 0,
                        Massege = ex.Message
                    });
                }
           
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        private async void ProccesUploadFile(UploadedFile uploadedFile)
        {

            ////////////////////////////////////
            //Take thumbnails

            //Thumbnails 1s
            await _artlistEngine.ProcessRequestThumbnails(new ProcessRequestThumbnails()
            {
                ProcessType = ProcessRequestType.CreateThumbnails,
                Id = Guid.NewGuid().ToString("N"),
                UploadFileId = uploadedFile.Id,
                Miliseconds = 1000,
                CallBackURL = _appSettings.ArtlisCoreServer.CallBackURL
            });

            //Thumbnails 3s
            await _artlistEngine.ProcessRequestThumbnails(new ProcessRequestThumbnails()
            {
                ProcessType = ProcessRequestType.CreateThumbnails,
                Id = Guid.NewGuid().ToString("N"),
                UploadFileId = uploadedFile.Id,
                Miliseconds = 3000,
                CallBackURL = _appSettings.ArtlisCoreServer.CallBackURL
            });


            //////////////////////////////////////
            ////Convert file to H264 codec
            await _artlistEngine.ProcessRequestConvert(new ProcessRequestConvert()
            {
                ProcessType = ProcessRequestType.ConvertFile,
                Id = Guid.NewGuid().ToString("N"),
                UploadFileId = uploadedFile.Id,
                Codec = SupportedCodecs.H264,
                CallBackURL = _appSettings.ArtlisCoreServer.CallBackURL
            });



        }
    }
}