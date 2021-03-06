﻿using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Model.Interfaces
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        DbContextTransaction BeginTransaction();
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, Boolean> predicate);
        TEntity FindById(int id);
        void Create(TEntity item);
        void Update(TEntity item);
        void Remove(TEntity item);
        void Remove(TEntity item, Func<TEntity, int> getKey);
        void Save();
    }
}
