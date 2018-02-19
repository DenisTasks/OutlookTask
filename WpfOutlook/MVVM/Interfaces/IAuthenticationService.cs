using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Interfaces
{
    public interface IAuthenticationService
    {
        UserDTO AuthenticateUser(string username, string password);
    }
}
