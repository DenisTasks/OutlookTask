using System;
using System.Collections.Generic;
using BLL.EntitesDTO;
using BLL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViewModel.ViewModels.Appointments;

namespace ViewModelTests.ViewModels.Appointments
{
    [TestClass]
    public class AddAppWindowViewModelTests
    {
        [TestMethod]
        public void CheckDatesOverlap()
        {
            //Arrange
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddHours(3);

            var startDateCompare = DateTime.Now.AddHours(1);
            var endDateCompare = DateTime.Now.AddHours(4);

            //Act
            bool overlap = startDate < endDateCompare && startDateCompare < endDate;

            //Assert
            Assert.IsTrue(overlap);
        }

        [TestMethod]
        public void LocationsCount()
        {
            //Arrange
            var mock = new Mock<IBLLServiceMain>();
            mock.Setup(s => s.GetLocations()).Returns(new List<LocationDTO>
            {
                new LocationDTO{LocationId = 1, Room = "TestRoom"}
            });

            AddAppWindowViewModel vm = new AddAppWindowViewModel(mock.Object);

            //Act
            int count = vm.GetLocationsCount();

            //Assert
            Assert.AreNotEqual(count, 0);
        }

        [TestMethod]
        public void UsersCount()
        {
            //Arrange
            var mock = new Mock<IBLLServiceMain>();
            mock.Setup(s => s.GetUsers()).Returns(new List<UserDTO>
            {
                new UserDTO { Name = "TestUser" }
            });

            AddAppWindowViewModel vm = new AddAppWindowViewModel(mock.Object);

            //Act
            int count = vm.GetUsersCount();

            //Assert
            Assert.AreNotEqual(count, 0);
        }

        [TestMethod]
        public void AppointmentIsNotNull()
        {
            var mock = new Mock<IBLLServiceMain>();
            AddAppWindowViewModel vm = new AddAppWindowViewModel(mock.Object);

            Assert.IsNotNull(vm.Appointment);
        }

        [TestMethod]
        public void SelectedDateTimeNow()
        {
            //Arrange
            var mock = new Mock<IBLLServiceMain>();

            //Act
            AddAppWindowViewModel vm = new AddAppWindowViewModel(mock.Object);

            //Assert
            Assert.AreEqual(vm.SelectedBeginningTime, vm.GetDateTimeNow());
        }
    }
}