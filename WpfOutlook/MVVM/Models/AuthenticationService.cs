using BLL.BLLService;
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
            using (var db = new BLLService())
            {
                UserDTO user = db.CheckUser(username, password);
                if (user != null)
                    return user;
                else throw new UnauthorizedAccessException("Wrong credentials.");
            }
        }
    }
}
