using BLL.DTO;

namespace TestWpf.Helpers
{
    public class OpenWindowMessage
    {
        public WindowType Type { get; set; }
        public string Argument { get; set; }
        public AppointmentDTO Appointment { get; set; }
    }
}
