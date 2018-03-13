using BLL.EntitesDTO;
using BLL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewModel.ViewModels.Authenication;

namespace ViewModelTests.ViewModels.authentications
{
    [TestFixture]
    public class AuthenticationViewModelTests
    {
        [TestCase]
        public void WrongCreditsTest()
        {
            var mock = new Mock<IAuthenticationService>();
            mock.Setup(s => s.AuthenticateUser("test", "password")).Throws(new UnauthorizedAccessException());

            var result = new AuthenticationViewModel(mock.Object).AuthenticatedUser;
            Assert.AreEqual(result, "You are not authenticated!");
        }

        //[TestCase]
        //public void AuthenticateUserTest()
        //{
        //    var mock = new Mock<IAuthenticationService>();
        //    CustomPrincipal.Sw
        //    mock.Setup(s => s.AuthenticateUser("test", "password")).Returns(new UserDTO { UserName = "test"});
        //    mock.Setup(s => s.GetRoles(1)).Returns(new string[]{"user"});



        //    var vm = new AuthenticationViewModel(mock.Object);
        //    var result = vm.AuthenticatedUser;
        //    Assert.AreEqual(result, "You are not authenticated!");
        //}
    }
}
