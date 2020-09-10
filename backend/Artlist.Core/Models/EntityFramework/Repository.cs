using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artlist.Core.Models.EntityFramework
{
    public abstract class Repository
    {
        private readonly DbContextOptions<ArtlistContext> _contextOptions;

        public Repository(DbContextOptions<ArtlistContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        protected ArtlistContext CreateContext()
        {
            return new ArtlistContext(_contextOptions);
        }

    }
}
