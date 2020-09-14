using Artlist.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Artlist.Common.Interfaces.Repository
{
    public interface IUploadFileRepository : IRepository<UploadedFile>
    {
        public Task<IList<UploadedFile>> GetLastAsync(int count,Boolean isWithChildren = false);
        public UploadedFile Get(string id, Boolean isWithChildren = false);
    }
}

