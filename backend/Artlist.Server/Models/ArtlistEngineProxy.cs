using Artlist.Common;
using Artlist.Common.Interfaces;
using Artlist.Common.Models;
using Artlist.Common.Models.ProcessTasks;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artlist.Server.Models
{
    public class ArtlistEngineProxy : IArtlistEngine
    {

        private readonly string UPLOAD_FILE_ENDPOINT = "api/v1/uploadedfiles";
        private readonly string CONVERT_ENDPOINT = "api/v1/convert";
        private readonly string TEMPORERY_ENDPOINT = "api/v1/temporeryfiles";
        private readonly string THUMBNAIL_ENDPOINT = "api/v1/thumbnails";
        private readonly string PROCESSTASK_ENDPOINT = "api/v1/processtask";

        private readonly AppSettings _appSettings;
        private readonly IFileStore _filestore;

        public ArtlistEngineProxy(AppSettings appSettings, IFileStore filestore)
        {
            _appSettings = appSettings;
            _filestore = filestore;
        }

       
        public async Task<UploadedFile> SaveFileAsync(Stream strem, string fileName)
        {

            TemporeryFile temporeryFile = null;
            UploadedFile uploadedFile = null;
            try
            {
                //The idea was to keep the file in temporary place as soon as possible
                //and the Artist.Core will process it later (we will acknowledge it)
                //we use the same resource - HDD in this case 
                temporeryFile = await _filestore.SaveTemporertFileAsync(strem, fileName);


                RestClient restClient = new RestClient($"{_appSettings.ArtlisCoreServer.URL}");

                RestRequest restRequest = new RestRequest(TEMPORERY_ENDPOINT, Method.POST);
                restRequest.RequestFormat = DataFormat.Json;

                restRequest.AddHeader("Content-Type", "application/json");

                restRequest.AddJsonBody(temporeryFile);

                var result = restClient.Execute(restRequest).Content;
                uploadedFile = JsonConvert.DeserializeObject<UploadedFile>(result);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Delete temporery file
                await _filestore.DeleteTemporertFileAsync(temporeryFile);

            }
            return uploadedFile;
        }


        public async Task ProcessRequestThumbnails(ProcessRequestThumbnails processTask)
        {

            RestClient restClient = new RestClient($"{_appSettings.ArtlisCoreServer.URL}");
            RestRequest restRequest = new RestRequest($"{PROCESSTASK_ENDPOINT}/thumbnails", Method.POST);
            restRequest.RequestFormat = DataFormat.Json;

            restRequest.AddHeader("Content-Type", "application/json");

            restRequest.AddJsonBody(processTask);
            //ProcessTask
            IRestResponse restResponse = restClient.Execute(restRequest);
            if (!restResponse.IsSuccessful)
            {
                throw new Exception(restResponse.ErrorMessage ?? restResponse.Content);
            }

            return;
        }

        public async Task ProcessRequestConvert(ProcessRequestConvert processTask)
        {
            RestClient restClient = new RestClient($"{_appSettings.ArtlisCoreServer.URL}");
            RestRequest restRequest = new RestRequest($"{PROCESSTASK_ENDPOINT}/convert", Method.POST);
            restRequest.RequestFormat = DataFormat.Json;

            restRequest.AddHeader("Content-Type", "application/json");

            restRequest.AddJsonBody(processTask);
            //ProcessTask
            IRestResponse restResponse = restClient.Execute(restRequest);
            if (!restResponse.IsSuccessful)
            {
                throw new Exception(restResponse.ErrorMessage ?? restResponse.Content);
            }

            return;
        }
    }
}
