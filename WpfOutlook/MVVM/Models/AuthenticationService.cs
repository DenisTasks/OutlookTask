using BLL.EntitesDTO;
using MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Models.Authenication
{
    public class AuthenticationService : IAuthenticationService
    {
        public UserDTO AuthenticateUser(string username, string password)
        {

            //UserDTO resultUser = _db.Users.Get(u => u.UserName.Equals(username) && u.Password.Equals(password)).FirstOrDefault();
            //if (resultUser != null)
            //    return resultUser;
            //else throw new UnauthorizedAccessException("Wrong credentials.");
            return null;
        }
    }
}
