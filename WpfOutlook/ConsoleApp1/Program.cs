using Model.Entities;
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
            using(UnitOfWork uw= new UnitOfWork())
            {
                Role myrole = new Role { Name = "dsadasda", RoleId = 1 };
                uw.Roles.Create(myrole);
                uw.Save();
                Console.WriteLine("Done");
            }
        }
    }
}
