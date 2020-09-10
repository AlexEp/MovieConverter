using System;
using System.Collections.Generic;

namespace Artlist.Common.Models
{
    public partial class Thumbnail
    {
        public string Id { get; set; }
        public string Filename { get; set; }
        public string SourceFileId { get; set; }
        public int FrameNum { get; set; }
        public DateTime Created { get; set; }

        public virtual UploadedFile SourceFiles { get; set; }
    }
}
