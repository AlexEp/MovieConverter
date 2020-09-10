using System;
using System.Collections.Generic;
using System.Text;

namespace Artlist.Common.Models.ProcessTasks
{
    public  abstract class Process
    {
        public string Id { get; set; }

        public ProcessRequestType ProcessType { get; set; }

        public string CallBackURL { get; set; }
    }

    public class ProcessRequestThumbnails : Process {
        public string UploadFileId { get; set; }
        public int  Miliseconds { get; set; }

    }

    public class ProcessRequestConvert : Process
    {
        public string UploadFileId { get; set; }
        public SupportedCodecs Codec { get; set; }
    }


    public class ProcessThumbnailsResponse : Process
    {
        public string UploadedFileId { get; set; }
        public ProcessRequestThumbnails  Request { get; set; }
        public ProcessStatusType Status { get; set; }
        public UploadedFile UploadedFile { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public string ErrorMassege { get; set; }
        public int ErrorCode { get; set; }

    }

    public class ProcessConvertResponse : Process
    {
        public string UploadedFileId { get; set; }
        public ProcessRequestConvert Request { get; set; }
        public ProcessStatusType Status { get; set; }
        public UploadedFile UploadedFile { get; set; }
        public ConvertedFile ConvertedFile { get; set; }
        public string ErrorMassege { get; set; }
        public int ErrorCode { get; set; }
    }
}
