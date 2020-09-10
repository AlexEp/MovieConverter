using Artlist.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Artlist.Common.Models
{
    public class HDDFileStore : IFileStore
    {
        private readonly string _baseFolder;

        public HDDFileStore(string baseFolder)
        {
            _baseFolder = baseFolder;
        }

        public async Task DeleteTemporertFileAsync(TemporeryFile file)
        {
            if (file != null)
            {
                bool fileExist = File.Exists(file.FileIdentifier);
                if (fileExist)
                {
                    var dirPath = Path.GetDirectoryName(file.FileIdentifier);
                    Directory.Delete(dirPath, true);
                }
            }

            return;
        }

        public async Task<TemporeryFile> SaveTemporertFileAsync(Stream strem, string fileName)
        {
            Guid guid = Guid.NewGuid();
            var currentDate = DateTime.UtcNow; //can be use to clean old file/folders
            var dirPath = Path.Combine(_baseFolder, "temporary", currentDate.ToString("yyyMMdd"));
            var fullPath = Path.Combine(dirPath, guid.ToString("N"));

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            //Save temporery
            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await strem.CopyToAsync(fileStream);
            }

            return new TemporeryFile() { Filename = fileName, FileIdentifier = fullPath, Created = currentDate };
        }

        public async Task<Stream> ReadTemporertFileAsync(TemporeryFile temporeryFile) {
            return new FileStream(temporeryFile.FileIdentifier, FileMode.Open, FileAccess.Read);
        }

        public async Task<UploadedFile> SaveUploadFileAsync(Stream strem, string fileName)
        {
            Guid guid = Guid.NewGuid();

            var uploadFile = new UploadedFile() { Id = guid.ToString("N"), Filename = fileName, Created = DateTime.UtcNow };

            var dirPath = GetUploadFileFolder(uploadFile);
            var fullPath = Path.Combine(dirPath, uploadFile.Id);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            //Save temporery
            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await strem.CopyToAsync(fileStream);
            }

            return uploadFile;
        }
        public string GetConvertedFileFolder(ConvertedFile file) {
            var currentDate = file.Created;
            string identifier = file.Id;
            string subFolder = identifier.Substring(0, Math.Min(3, identifier.Length));
            var dirPath = Path.Combine(_baseFolder, "converted_file", currentDate.ToString("yyyMMdd"), subFolder);

            return dirPath;
        }
        public string GetUploadFileFolder(UploadedFile file) {
            var currentDate = file.Created; 
            string identifier = file.Id;
            string subFolder = identifier.Substring(0, Math.Min(3, identifier.Length));
            var dirPath = Path.Combine(_baseFolder, "uploaded_file", currentDate.ToString("yyyMMdd"), subFolder);

            return dirPath;
        }

        public async Task DeleteUploadFileAsync(UploadedFile file)
        {
            if (file != null)
            {
                var dirPath = GetUploadFileFolder(file);

                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                }
            }

            return;
        }

        public string GetThumbnailFileFolder(Thumbnail file)
        {
            var currentDate = file.Created;
            string identifier = file.Id;
            string subFolder = identifier.Substring(0, Math.Min(3, identifier.Length));
            var dirPath = Path.Combine(_baseFolder, "thumbnails", currentDate.ToString("yyyMMdd"), subFolder);

            return dirPath;
        }
    }
}
