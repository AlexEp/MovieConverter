using Artlist.Common.Interfaces.Repository;
using Artlist.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artlist.Core.Models.EntityFramework
{
    public class ThumbnailFileRepository : Repository , IThumbnailFileRepository
    {

        public ThumbnailFileRepository(DbContextOptions<ArtlistContext> contextOptions) : base(contextOptions)
        {

        }

        public async void Delete(Thumbnail entityToDelete)
        {
            if (entityToDelete != null)
            {
                using (var context = CreateContext())
                {
                    context.Thumbnails.Remove(entityToDelete);
                    await context.SaveChangesAsync();
                }
            }
        }

        public void Delete(string id)
        {
            Thumbnail file = Get(id);

            if (file != null)
            {
                Delete(file);
            }
        }

        public Thumbnail Get(string id)
        {
            Thumbnail thumbnail = null;
            using (var context = CreateContext())
            {
                thumbnail = context.Thumbnails.FirstOrDefault(t => t.Id == id);
            }

            return thumbnail;
        }



        public async void Insert(Thumbnail entity)
        {
            using (var context = CreateContext())
            {
                await context.Thumbnails.AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }

        public void Update(Thumbnail entity)
        {
            throw new NotImplementedException();
        }
    }
}
