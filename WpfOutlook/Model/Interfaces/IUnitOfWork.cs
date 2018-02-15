using Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IUnitOfWork: IDisposable 
    {
        IGenericRepository<Appointment> Appointments { get; }
        IGenericRepository<Group> Groups { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<Location> Locations { get; }
        DbContextTransaction BeginTransaction();
        void Save();
    }
}
