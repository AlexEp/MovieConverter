using Artlist.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artlist.Server.Models
{
    public class UploadedFileResponse
    {
  

        public UploadedFileResponse(UploadedFile uf)
        {
            this.Filename = uf.Filename;
            this.Created = uf.Created;
            this.Id = uf.Id;
        }

        public string Filename { get;  set; }
        public DateTime Created { get; set; }
        public string Id { get;  set; }


    }
}
