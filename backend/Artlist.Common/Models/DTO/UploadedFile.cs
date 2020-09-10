using System;
using System.Collections.Generic;

namespace Artlist.Common.Models
{
    public partial class UploadedFile
    {
        public UploadedFile()
        {
            ConvertedFiles = new HashSet<ConvertedFile>();
            Thumbnails = new HashSet<Thumbnail>();
        }

        public string Id { get; set; }
        public string Filename { get; set; }
        public string Hashed { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<ConvertedFile> ConvertedFiles { get; set; }
        public virtual ICollection<Thumbnail> Thumbnails { get; set; }
    }
}
