using Artlist.Common.Interfaces.Repository;
using Artlist.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artlist.Core.Models.EntityFramework
{
    public class ConvertedFileRepository : Repository , IConvertedFileRepository
    {

        public ConvertedFileRepository(DbContextOptions<ArtlistContext> contextOptions) : base(contextOptions)
        {

        }

        public async void Delete(Common.Models.ConvertedFile entityToDelete)
        {
            if (entityToDelete != null)
            {
                using (var context = CreateContext())
                {
                    context.ConvertedFiles.Remove(entityToDelete);
                    await context.SaveChangesAsync();
                }
            }
        }

        public void Delete(string id)
        {
            ConvertedFile file = Get(id);

            if (file != null)
            {
                    Delete(file);
            }
        }

        public  ConvertedFile Get(string id)
        {
            ConvertedFile convertedFile = null;
            using (var context = CreateContext())
            {
                convertedFile = context.ConvertedFiles.FirstOrDefault(cf => cf.Id == id);
            }

            return convertedFile;
        }

        public async void Insert(Common.Models.ConvertedFile entity)
        {
            using (var context = CreateContext())
            {
                await context.ConvertedFiles.AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }

        public void Update(Common.Models.ConvertedFile entity)
        {
            throw new NotImplementedException();
        }
    }
}
