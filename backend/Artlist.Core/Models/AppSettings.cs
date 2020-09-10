using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artlist.Core.Models
{
    public class AppSettings
    {
        public FileSettings Files { get; set; }
        public DatabaseSettings Database { get; set; }
        public FFmpegSettings FFmpeg { get; set; }
     
    }

    
   public class FFmpegSettings
    {
        public string FFmpegFolder { get; set; }
    }

    public class FileSettings
    {
        public string BaseFolder { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }
}
