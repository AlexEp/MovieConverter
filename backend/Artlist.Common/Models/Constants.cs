using System;
using System.Collections.Generic;
using System.Text;

namespace Artlist.Common.Models
{
    public enum SupportedCodecs
    {
        H264,
        MultibitHLS
    }

    public enum ProcessRequestType
    {
        FileUpload,
        FullProcess,
        ConvertFile,
        CreateThumbnails
    }

    public enum ProcessStatusType
    {
        Started,
        Completed,
        InProcess,
        Failed

    }


}
