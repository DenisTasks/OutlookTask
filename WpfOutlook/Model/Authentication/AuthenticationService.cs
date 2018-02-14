using Model.Entities;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private WPFOutlookContext _context;

        public AuthenticationService()
        {
            _context = new WPFOutlookContext();
        }

        public User AuthenticateUser(string username, string password)
        {
            User resultUser = _context.Users.FirstOrDefault(u => u.UserName.Equals(username) && u.Password.Equals(password));
            if (resultUser != null)
                return resultUser;
            else throw new UnauthorizedAccessException("Wrong credentials.");
        }
    }
}
