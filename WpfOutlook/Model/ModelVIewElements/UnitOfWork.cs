using Model.Entities;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ModelVIewElements
{
    public class UnitOfWork : IUnitOfWork
    {
        private WPFOutlookContext _context;
        private IGenericRepository<Appointment> _appointments;
        private IGenericRepository<Group> _groups;
        private IGenericRepository<User> _users;
        private IGenericRepository<Role> _roles;
        private IGenericRepository<Location> _location;

        public UnitOfWork()
        {
            _context = new WPFOutlookContext();
        }

        public DbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        public IGenericRepository<Appointment> Appointments
        {
            get
            {
                return _appointments ?? new GenericRepository<Appointment>(_context);
            }
        }

        public IGenericRepository<Group> Groups
        {
            get
            {
                return _groups ?? new GenericRepository<Group>(_context);
            }
        }

        public IGenericRepository<User> Users
        {
            get
            {
                return _users ?? new GenericRepository<User>(_context);
            }
        }

        public IGenericRepository<Role> Roles 
        {
            get
            {
                return _roles ?? new GenericRepository<Role>(_context);
            }   
        }

        public IGenericRepository<Location> Locations
        {
            get
            {
                return _location ?? new GenericRepository<Location>(_context);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
