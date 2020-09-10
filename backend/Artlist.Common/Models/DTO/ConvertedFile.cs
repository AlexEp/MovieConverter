using System;
using System.Collections.Generic;

namespace Artlist.Common.Models
{
    public partial class ConvertedFile
    {
        public string Id { get; set; }
        public string SourceFilesId { get; set; }
        public DateTime Created { get; set; }
        public string Codec { get; set; }

        public virtual UploadedFile SourceFiles { get; set; }
    }
}
