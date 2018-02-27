﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, Boolean> predicate);
        TEntity FindById(int id);
        IQueryable<TEntity> FindBy(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] include);
        void Create(TEntity item);
        void Update(TEntity item);
        void Remove(TEntity item);
    }
}
