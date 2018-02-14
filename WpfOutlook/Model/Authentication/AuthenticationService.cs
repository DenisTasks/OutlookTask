using Model.Entities;
using Model.Interfaces;
using Model.ModelVIewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUnitOfWork _db;

        public AuthenticationService()
        {
            _db = new UnitOfWork();
        }

        public User AuthenticateUser(string username, string password)
        {
            User resultUser = _db.Users.Get(u => u.UserName.Equals(username) && u.Password.Equals(password)).FirstOrDefault();
            if (resultUser != null)
                return resultUser;
            else throw new UnauthorizedAccessException("Wrong credentials.");
            
        }
    }
}
