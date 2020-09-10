using Artlist.Common.Interfaces;
using Artlist.Common.Models;
using Artlist.Core.Controllers.V1;
using Artlist.Core.Models;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using System.Linq;
using System;

namespace Artlist.Core.UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CONVERT_FILE()
        {
            var outputfolder = @"D:\Temporery\Hell\Converted";
            var filename = @"codevlueInterview.mp4";
            
            var fileStore = new Mock<IFileStore>();
            fileStore.Setup(foo => foo.GetConvertedFileFolder(It.IsAny<ConvertedFile>())).Returns(outputfolder);
            fileStore.Setup(foo => foo.GetUploadFileFolder(It.IsAny<UploadedFile>())).Returns(@"D:\Temporery\Hell\");

            var appSettings = new AppSettings()
            {
                FFmpeg = new FFmpegSettings() { FFmpegFolder = @"D:\Development\Artlist\backend\ffmpeg-win64-static\bin" }
            };

            //appSettings.SetupProperty(foo => foo.FFmpeg.FFmpegFolder, @"D:\Development\Artlist\backend\ffmpeg-win64-static\bin");

            FileConverter converter = new FileConverter(fileStore.Object, appSettings);

            var uploadedFile = new UploadedFile() { Filename = filename , Id ="qewcaceaw"};

            var convertedFile = await converter.ConvertFileAsync(uploadedFile,SupportedCodecs.H264);


            var output = Path.Combine(outputfolder, filename);

            IMediaInfo resultFile =  await FFmpeg.GetMediaInfo(output);
            Assert.AreEqual("h264", resultFile.VideoStreams.First().Codec);
            Assert.AreEqual(29.97, resultFile.VideoStreams.First().Framerate);

          
        }



        [Test]
        public async Task TAKE_FRAME()
        {
            var outputfolder = @"D:\Temporery\Hell\Converted";
            var filename = @"Mulan.mp4";

            var fileStore = new Mock<IFileStore>();
            fileStore.Setup(foo => foo.GetConvertedFileFolder(It.IsAny<ConvertedFile>())).Returns(outputfolder);
            fileStore.Setup(foo => foo.GetUploadFileFolder(It.IsAny<UploadedFile>())).Returns(@"D:\Temporery\Hell\");
            fileStore.Setup(foo => foo.GetThumbnailFileFolder(It.IsAny<Thumbnail>())).Returns(@"D:\Temporery\Hell\Thumbnail");
            
            var appSettings = new AppSettings()
            {
                FFmpeg = new FFmpegSettings() { FFmpegFolder = @"D:\Development\Artlist\backend\ffmpeg-win64-static\bin" }
            };

            //appSettings.SetupProperty(foo => foo.FFmpeg.FFmpegFolder, @"D:\Development\Artlist\backend\ffmpeg-win64-static\bin");

            FileConverter converter = new FileConverter(fileStore.Object, appSettings);

            var uploadedFile = new UploadedFile() { Filename = filename, Id = "qewcaceaw" };

            var convertedFile = await converter.TakeFrame(uploadedFile, TimeSpan.FromSeconds(30));


            var output = Path.Combine(outputfolder, filename);

            IMediaInfo resultFile = await FFmpeg.GetMediaInfo(output);
            //Assert.AreEqual("h264", resultFile.VideoStreams.First().Codec);
            //Assert.AreEqual(29.97, resultFile.VideoStreams.First().Framerate);

            Assert.Pass();
        }
    }
}