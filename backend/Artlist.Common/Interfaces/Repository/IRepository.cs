using System;
using System.Collections.Generic;
using System.Text;

namespace Artlist.Common.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);
        void Delete(string id);
        TEntity Get(string id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
    }
}
