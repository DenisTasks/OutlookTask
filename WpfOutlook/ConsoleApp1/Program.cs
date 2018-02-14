using Model.Entities;
using Model.Interfaces;
using Model.ModelVIewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IUnitOfWork uw = new UnitOfWork())
            {
                uw.Roles.Create(new Role { RoleId = 1, Name = "hello" });
            }
        }
    }
}
