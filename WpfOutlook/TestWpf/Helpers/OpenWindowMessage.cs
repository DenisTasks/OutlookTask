using Model.Entities;

namespace TestWpf.Helpers
{
    public class OpenWindowMessage
    {
        public WindowType Type { get; set; }
        public string Argument { get; set; }
        public Appointment Appointment { get; set; }
    }
}
