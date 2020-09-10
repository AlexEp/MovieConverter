using Artlist.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Artlist.Common.Interfaces
{
    public interface IFileStore
    {
        public Task<TemporeryFile> SaveTemporertFileAsync(Stream strem,string fileName);
        public Task<UploadedFile> SaveUploadFileAsync(Stream strem, string fileName);

        public Task DeleteTemporertFileAsync(TemporeryFile file);

        public  Task<Stream> ReadTemporertFileAsync(TemporeryFile temporeryFile);

        public Task DeleteUploadFileAsync(UploadedFile file);
        public string GetUploadFileFolder(UploadedFile file);
        public string GetConvertedFileFolder(ConvertedFile file);
        public string GetThumbnailFileFolder(Thumbnail thumbnail);
    }
}
