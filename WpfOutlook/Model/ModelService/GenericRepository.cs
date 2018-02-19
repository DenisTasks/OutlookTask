
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ModelService
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class
    {
        private WPFOutlookContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(WPFOutlookContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Create(TEntity item)
        {
            _dbSet.Add(item);
        }

        public void Update(TEntity item)
        {
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Remove(TEntity item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _dbSet.Attach(item);
            }
            _dbSet.Remove(item);
        }
    }
}
