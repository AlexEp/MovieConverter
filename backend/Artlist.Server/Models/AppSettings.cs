using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artlist.Server.Models
{
    public class AppSettings
    {
        public FileSettings Files { get; set; }
        public ArtlisCoreServerSettings ArtlisCoreServer { get; set; }
        public CORSSettings CORS { get; set; }
    }

    
    public class ArtlisCoreServerSettings
    {
        public string URL { get; set; }
        public string CallBackURL { get; set; }
    }

    public class CORSSettings
    {
        public string FrontEndURL { get; set; }
    }
    public class FileSettings
    {
        public string BaseFolder { get; set; }
    }
}
