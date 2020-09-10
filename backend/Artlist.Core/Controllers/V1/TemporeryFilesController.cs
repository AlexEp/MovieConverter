using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Artlist.Common.Interfaces;
using Artlist.Common.Interfaces.Repository;
using Artlist.Common.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Artlist.Core.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class TemporeryFilesController : ControllerBase
    {

        private readonly IArtlistEngine _artlistEngine;
        private readonly IFileStore _fileStore;
        private readonly IUploadFileRepository _uploadFileRepository;

        public TemporeryFilesController(IArtlistEngine artlistEngine, IFileStore fileStore, IUploadFileRepository uploadFileRepository)
        {

            _artlistEngine = artlistEngine;
            _fileStore = fileStore;
            _uploadFileRepository = uploadFileRepository;
        }


        [HttpPost]
        [RequestSizeLimit(209715200)]
        public async Task<UploadedFile> Post([FromBody]TemporeryFile temporeryFile)
        {
            UploadedFile uploadFile = null;
            using (Stream file = await _fileStore.ReadTemporertFileAsync(temporeryFile)) {

                uploadFile = await _fileStore.SaveUploadFileAsync(file, temporeryFile.Filename);
            }

            try
            {
                _uploadFileRepository.Insert(uploadFile);
            }
            catch (Exception)
            {
                await _fileStore.DeleteUploadFileAsync(uploadFile);
                throw;
            }
         
        
            return uploadFile;
        }
    
    }
}
