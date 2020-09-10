using Artlist.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artlist.Core.Models
{
    public interface IFileConverter
    {
        Task<ConvertedFile> ConvertFileAsync(UploadedFile uploadedFile, SupportedCodecs codecs);
        Task<Thumbnail> TakeFrame(UploadedFile uploadedFile,TimeSpan span);
    }
}
