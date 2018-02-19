using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Interfaces;
using Model.ModelVIewElements;
using Moq;
using ViewModel.ViewModels;

namespace ViewModelTests
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void CanExecuteAboutAppointmentCommand()
        {
            //Arrange
            var mock = new Mock<IBLLService>();
            mock.Setup(a => a.GetAppointmentById(1)).Returns(new AppointmentDTO());
            MainWindowViewModel vm = new MainWindowViewModel(mock.Object);
            AppointmentDTO appointment = new AppointmentDTO();
            ICommand command = vm.AboutAppointmentCommand;

            //Act
            //Assert
            command.Execute(appointment);
        }
    }
}
