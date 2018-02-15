using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;

namespace ViewModel.Interfaces
{
    public interface IBLLService : IDisposable
    {
        IEnumerable<Appointment> GetAppointments();
        void AddAppointment(Appointment appointment);
        void RemoveAppointment(Appointment appointment);
    }
}
