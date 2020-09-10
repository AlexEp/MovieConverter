using Artlist.Common.Interfaces.Repository;
using Artlist.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artlist.Core.Models.EntityFramework
{
    public class UploadFileRepository : Repository , IUploadFileRepository
    {

        public UploadFileRepository(DbContextOptions<ArtlistContext> contextOptions) : base(contextOptions)
        {

        }

     
        public async void Delete(Common.Models.UploadedFile entityToDelete)
        {
            if (entityToDelete != null)
            {
                using (var context = CreateContext())
                {
                    context.UploadedFiles.Remove(entityToDelete);
                    await context.SaveChangesAsync();
                }
            }
        }

        public void Delete(string id)
        {
            UploadedFile file = Get(id);

            if (file != null)
            {
                Delete(file);
            }
        }

        public  UploadedFile Get(string id)
        {
            UploadedFile file;
            using (var context = CreateContext())
            {
                file =  context.UploadedFiles.FirstOrDefault(up => up.Id == id);
            }

            return file;
        }

        public async Task<IList<UploadedFile>> GetLastAsync(int count)
        {
            IList<UploadedFile> list;
            using (var context = CreateContext())
            {
                    list = await context.UploadedFiles.OrderBy(up => up.Created).Take(count).ToListAsync();
            }

            return list;
        }



        public async void Insert(Common.Models.UploadedFile entity)
        {
            using (var context = CreateContext()) {
                await context.UploadedFiles.AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }

        public void Update(Common.Models.UploadedFile entity)
        {
            throw new NotImplementedException();
        }
    }
}
