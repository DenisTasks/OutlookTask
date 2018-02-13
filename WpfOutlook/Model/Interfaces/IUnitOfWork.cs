using Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IUnitOfWork<TEntity> : INotifyPropertyChanged where TEntity: class
    {
        IGenericRepository<Appointment> Managers { get; }
        IGenericRepository<Group> Sales { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<Location> Locations { get; }
        void Save();
    }
}
