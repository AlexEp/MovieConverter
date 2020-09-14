using Artlist.Common.Models;
using Artlist.Common.Models.ProcessTasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Artlist.Common.Interfaces
{
    public interface IArtlistEngine
    {
        public Task<UploadedFile> SaveFileAsync(Stream file,string fileName);

        //public Task<ConvertedFile> ConvertFileAsync(UploadedFile uploadedFile, SupportedCodecs codecs);

        //public  Task<IList<UploadedFile>> GetLastUplodedFiles(int amout);
        //public Task<Thumbnail> TakeThumbnail(UploadedFile uploadedFile, TimeSpan timeSpan);

         Task ProcessRequestThumbnails(ProcessRequestThumbnails processTask);
         Task ProcessRequestConvert(ProcessRequestConvert processTask);

        Task<Stream> GetThumbnails(string id);



    }
}
