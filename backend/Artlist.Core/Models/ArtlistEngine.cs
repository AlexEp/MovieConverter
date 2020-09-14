using Artlist.Common.Interfaces;
using Artlist.Common.Interfaces.Repository;
using Artlist.Common.Models;
using Artlist.Common.Models.ProcessTasks;
using Artlist.Core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Artlist.Core
{
    public class ArtlistEngine  : IArtlistEngine
    {
        private readonly string CONVERT_ENDPOINT = "api/v1/callback/fileconvert";
        private readonly string THUMBNAIL_ENDPOINT = "api/v1/callback/thumbnails";


        private readonly string _filesPath;
        private readonly IFileStore _fileStore;
        private readonly IFileConverter _fileConverter;
        private readonly ILogger<ArtlistEngine> _logger;
        private readonly IUploadFileRepository _uploadFileRepository;
        private readonly IConvertedFileRepository _convertedFileRepository;
        private readonly IThumbnailFileRepository _thumbnailFileRepository;


        public ArtlistEngine(string filesPath, 
            IUploadFileRepository uploadFileRepository, 
            IFileStore fileStore, IFileConverter fileConvrter,ILogger<ArtlistEngine> logger,
            IConvertedFileRepository convertedFileRepository,
            IThumbnailFileRepository thumbnailFileRepository)
        {
            _uploadFileRepository = uploadFileRepository;
            _convertedFileRepository = convertedFileRepository;
            _thumbnailFileRepository = thumbnailFileRepository;

            _filesPath = filesPath;
            _fileStore = fileStore;
            _fileConverter = fileConvrter;
            _logger = logger;
        }

        public async Task<UploadedFile>  SaveFileAsync(Stream stream,string fileName) {

            var uploadedFile = await _fileStore.SaveUploadFileAsync(stream, fileName);

            try
            {
                _uploadFileRepository.Insert(uploadedFile);
            }
            catch (Exception)
            {
                await _fileStore.DeleteUploadFileAsync(uploadedFile);
                throw;
            }
        

            return uploadedFile;

        }


        public async Task ProcessRequestThumbnails(ProcessRequestThumbnails processTask)
        {

            ProcessThumbnailsResponse response = new ProcessThumbnailsResponse();
            response.Id = Guid.NewGuid().ToString("N");
            response.ProcessType = ProcessRequestType.CreateThumbnails;
            response.Status = ProcessStatusType.Started;
            response.Request = processTask;
            response.UploadedFileId = processTask.UploadFileId;

            UploadedFile uploadedFile;
            Thumbnail thumbnail = new Thumbnail();

            try
            {
                SendThumbnailsCallBack(response, processTask); //Start

                uploadedFile = _uploadFileRepository.Get(processTask.UploadFileId);
                response.UploadedFile = uploadedFile;

                thumbnail = await _fileConverter.TakeFrame(uploadedFile, TimeSpan.FromMilliseconds( processTask.Miliseconds));
                response.Thumbnail = thumbnail;
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message, exp, processTask);
                response.Status = ProcessStatusType.Failed;
                response.ErrorMassege = exp.Message;
            }



            if (thumbnail != null && !string.IsNullOrEmpty(thumbnail.Id))
            {
                response.Status = ProcessStatusType.Completed;

                try
                {
                    _thumbnailFileRepository.Insert(thumbnail);
                }
                catch (Exception exp)
                {

                    _logger.LogError("Erron on insert thumbnail info to DB", exp.Message);
                }
             
                SendThumbnailsCallBack(response, processTask); //Completed
            }
            else
            {
                response.Status = ProcessStatusType.Failed;

                SendThumbnailsCallBack(response, processTask); //Failed
            }

        }

        private void SendThumbnailsCallBack(ProcessThumbnailsResponse response, ProcessRequestThumbnails processTask)
        {
            RestClient restClient = new RestClient($"{processTask.CallBackURL}");
            RestRequest restRequest = new RestRequest(THUMBNAIL_ENDPOINT, Method.POST);

            restRequest.RequestFormat = DataFormat.Json;

            restRequest.AddHeader("Content-Type", "application/json");

            string json = JsonConvert.SerializeObject(response);
            restRequest.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse restResponse = restClient.Execute(restRequest);


            if (!restResponse.IsSuccessful)
            {
                _logger.LogError("Faild to send ThumbnailsResponse callback",  restResponse.Content, processTask);
                // throw new Exception(restResponse.ErrorMessage ?? restResponse.Content);
            }
        }

        public async Task ProcessRequestConvert(ProcessRequestConvert processTask)
        {

            ProcessConvertResponse response = new ProcessConvertResponse();
            response.Id = Guid.NewGuid().ToString("N");
            response.ProcessType = ProcessRequestType.ConvertFile;
            response.Status = ProcessStatusType.Started;
            response.Request = processTask;
            response.UploadedFileId = processTask.UploadFileId;

            UploadedFile uploadedFile;
            ConvertedFile convertedFile = new ConvertedFile(); 

            try
            {
                SendFileConvertCallBack(response, processTask); //Start

                uploadedFile = _uploadFileRepository.Get(processTask.UploadFileId);
                response.UploadedFile = uploadedFile;

                convertedFile = await _fileConverter.ConvertFileAsync(uploadedFile, processTask.Codec);
                response.ConvertedFile = convertedFile;
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message, exp, processTask);
                response.Status = ProcessStatusType.Failed;
                response.ErrorMassege = exp.Message;
            }
          
      

            if (convertedFile != null && !string.IsNullOrEmpty( convertedFile.Id))
            {

                response.Status = ProcessStatusType.Completed;

                try
                {
                    _convertedFileRepository.Insert(convertedFile);
                }
                catch (Exception exp)
                {

                    _logger.LogError("Erron on insert convertedFile info to DB", exp.Message);
                }

                SendFileConvertCallBack(response, processTask); //Completed
            }
            else
            {
                response.Status = ProcessStatusType.Failed;

                SendFileConvertCallBack(response, processTask); //Failed
            }
           


        }

        private void SendFileConvertCallBack(ProcessConvertResponse response, ProcessRequestConvert processTask) {
            RestClient restClient = new RestClient($"{processTask.CallBackURL}");
            RestRequest restRequest = new RestRequest(CONVERT_ENDPOINT, Method.POST);

            restRequest.RequestFormat = DataFormat.Json;

            restRequest.AddHeader("Content-Type", "application/json");

            string json = JsonConvert.SerializeObject(response);
            restRequest.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse restResponse = restClient.Execute(restRequest);


            if (!restResponse.IsSuccessful)
            {
                _logger.LogError("Faild to send ConvertResponse callback", restResponse.Content, processTask);
                // throw new Exception(restResponse.ErrorMessage ?? restResponse.Content);
            }
        }

        public async Task<Stream> GetThumbnails(string id)
        {
            var thumbnail = _thumbnailFileRepository.Get(id);

            if (thumbnail == null)
            {
                throw new Exception($"File {id} not Found");
            }
            var path = _fileStore.GetThumbnailFileFolder(thumbnail);
            var pathSource = Path.Combine(path,$"{thumbnail.Id}.png");

            FileStream fsSource = new FileStream(pathSource,
                FileMode.Open, FileAccess.Read);

            return fsSource;
        }
    }
}
