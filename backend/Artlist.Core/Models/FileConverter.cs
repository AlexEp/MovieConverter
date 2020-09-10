using Artlist.Common.Interfaces;
using Artlist.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace Artlist.Core.Models
{
    public class FileConverter : IFileConverter
    {
        private readonly IFileStore _fileStore;
        private readonly AppSettings _appSettings;

        public FileConverter(IFileStore fileStore, AppSettings appSettings)
        {
            this._fileStore = fileStore;
            this._appSettings = appSettings;


            FFmpeg.SetExecutablesPath(appSettings.FFmpeg.FFmpegFolder);
        } 
        public async Task<ConvertedFile> ConvertFileAsync(UploadedFile uploadedFile, SupportedCodecs codecs)
        {
            Guid guid = Guid.NewGuid();
            var convertedFile = new ConvertedFile() { Id = guid.ToString("N"),
                SourceFilesId = uploadedFile.Id,
                Created = DateTime.UtcNow,
                Codec = Enum.GetName(typeof(SupportedCodecs),codecs)
            };

            var uploadedFileFolder = _fileStore.GetUploadFileFolder(uploadedFile);
            var convertedFileFileFolder = _fileStore.GetConvertedFileFolder(convertedFile);

            if (!Directory.Exists(convertedFileFileFolder))
            {
                Directory.CreateDirectory(convertedFileFileFolder);
            }

            var fileToConvert = Path.Combine(uploadedFileFolder, uploadedFile.Id);
            var outputFileName = Path.Combine(convertedFileFileFolder, $"{uploadedFile.Id}{Path.GetExtension( uploadedFile.Filename)}");


            IMediaInfo mediaInfo;
            try
            {
                mediaInfo = await FFmpeg.GetMediaInfo(fileToConvert);
            }
            catch (Exception)
            {

                throw new ArgumentException($"Failed to Thumbnail. Invalid file type");
            }


            var videoStream = mediaInfo.VideoStreams.First();

            if (codecs == SupportedCodecs.H264)
            {
                videoStream.SetCodec(VideoCodec.h264);
            }
            

            var audioStream = mediaInfo.AudioStreams.First();

            var conversion = FFmpeg.Conversions.New();
            conversion.AddStream(videoStream);
            conversion.AddStream(audioStream);
            conversion.SetOutput(outputFileName).SetOverwriteOutput(true).UseMultiThread(true);

            // var conversion = await FFmpeg.Conversions.FromSnippet.Convert(fileToConvert, outputFileName);
            await conversion.Start();


            return convertedFile;


        }

        public async Task<Thumbnail> TakeFrame(UploadedFile uploadedFile, TimeSpan span)
        {
            Guid guid = Guid.NewGuid();

            var thumbnail = new Thumbnail() {
                Id = guid.ToString("N"),
                Filename = Path.ChangeExtension(uploadedFile.Filename, "png"),
                SourceFileId = uploadedFile.Id, 
                Created = DateTime.UtcNow };

            var uploadedFileFolder = _fileStore.GetUploadFileFolder(uploadedFile);
            var convertedFileFileFolder = _fileStore.GetThumbnailFileFolder(thumbnail);

            if (!Directory.Exists(convertedFileFileFolder))
            {
                Directory.CreateDirectory(convertedFileFileFolder);
            }

            var fileToConvert = Path.Combine(uploadedFileFolder, uploadedFile.Id);
            var outputFileName = Path.Combine(convertedFileFileFolder, uploadedFile.Id);

            IMediaInfo mediaInfo;
            try
            {
                mediaInfo = await FFmpeg.GetMediaInfo(fileToConvert);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Failed to Thumbnail. Invalid file type");
            }
          


            if (mediaInfo.Duration < span)
            {
                throw new ArgumentException($"Can't take a Thumbnail of {span}, the video is too short");
            }
            IVideoStream videoStream = mediaInfo.VideoStreams.First().SetCodec(VideoCodec.png);
            Func<string, string> outputBuilder = (number) => { return GetPath(convertedFileFileFolder, number, thumbnail); };

            thumbnail.FrameNum = (int)(videoStream.Framerate * span.TotalSeconds);

            try
            {
                IConversionResult conversionResult = await FFmpeg.Conversions.New()
                                                       .AddStream(videoStream) //.SetSeek(span)
                                                       .ExtractNthFrame(thumbnail.FrameNum, outputBuilder)
                                                       .Start();
            }
            catch (Exception)
            {

                throw;
            }
     

            return thumbnail;
        }

        private string GetPath(string convertedFileFileFolder,string num, Thumbnail thumbnail) {
            return Path.Combine(convertedFileFileFolder, $"{thumbnail.Id}.png");
        }
    }
}
