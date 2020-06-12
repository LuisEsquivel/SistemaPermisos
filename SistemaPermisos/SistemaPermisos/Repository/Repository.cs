using SistemaPermisos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SistemaPermisos
{
    public class Repository<TEntity> where TEntity : class
    {
        protected readonly Func<DbContext> _dbContextCreator;

        public Repository(Func<DbContext> dbContextCreator)
        {
            if (dbContextCreator == null) throw new ArgumentNullException(nameof(dbContextCreator), $"The parameter dbContextCreator can not be null");

            _dbContextCreator = dbContextCreator;
        }



        public IEnumerable<TEntity> ListAll()
        {
            var result = Enumerable.Empty<TEntity>();

            using (var context = _dbContextCreator())
            {
                var dbSet = context.Set<TEntity>();

                result = dbSet.ToList();
            }

            return result;
        }

        public int Add(TEntity newEntity)
        {
            if (newEntity == null) throw new ArgumentNullException(nameof(newEntity), $"The parameter newEntity can not be null");

            var result = 0;

            using (var context = _dbContextCreator())
            {
                var dbSet = context.Set<TEntity>();

                dbSet.Add(newEntity);

                result = context.SaveChanges();
            }

            return result;
        }


        public int Update(TEntity updateEntity)
        {
            if (updateEntity == null) throw new ArgumentNullException(nameof(updateEntity), $"The parameter updateEntity can not be null");

            var result = 0;

            using (var context = _dbContextCreator())
            {
                var dbSet = context.Set<TEntity>();

                dbSet.Attach(updateEntity);

                context.Entry(updateEntity).State = EntityState.Modified;

                result = context.SaveChanges();
            }

            return result;
        }




        public int Remove(TEntity removeEntity)
        {
            if (removeEntity == null) throw new ArgumentNullException(nameof(removeEntity), $"The parameter removeEntity can not be null");

            var result = 0;

            using (var context = _dbContextCreator())
            {
                var dbSet = context.Set<TEntity>();

                dbSet.Attach(removeEntity);

                context.Entry(removeEntity).State = EntityState.Deleted;

                result = context.SaveChanges();
            }

            return result;
        }




    }
}