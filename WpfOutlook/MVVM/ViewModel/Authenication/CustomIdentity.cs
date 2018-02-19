using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModel.Authenication
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, string username, string[] roles)
        {
            Name = name;
            Username = username;
            Roles = roles;
        }

        public string Name { get; private set; }
        public string Username { get; private set; }
        public string[] Roles { get; private set; }

        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
    }
}
