using GalaSoft.MvvmLight;
using Model.Entities;
using Model.ModelVIewElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ViewModel.Authentication;
using ViewModel.Interfaces;

namespace ViewModel.Print
{
    public class PrintAppointmentViewModel: ViewModelBase, IViewModel
    {
        private Appointment _appointment;
        private DelegateCommand _print;

        public PrintAppointmentViewModel()
        {
            _print = new DelegateCommand(ButtonPrintPressed, null);
            using (var uow = new UnitOfWork())
            {
                _appointment = uow.Appointments.FindById(9);
            }
        }

        public DelegateCommand PrintCommand { get { return _print; } }

        //public PrintAppointmentViewModel(int id)
        //{
        //    using (var uow = new UnitOfWork())
        //    {
        //        _appointment = uow.Appointments.FindById(id);
        //    }
        //}

        public string AppointmentName
        {
            get { return _appointment.Subject; }
        }

        public DateTime AppointmentBeginDate
        {
            get { return _appointment.BeginningDate; }
        }

        public int AppointmentId
        {
            get { return _appointment.AppointmentId; }
        }

        private void ButtonPrintPressed(object parametr)
        {
            PrintDialog myPrintDialog = new PrintDialog();

            if (myPrintDialog.ShowDialog() == true)

            {

                myPrintDialog.PrintVisual(this, "Form All Controls Print");

            }



            //open the pdf file
            //FixedDocument fixedDocument;
            //using (FileStream pdfFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
            //{
            //    Document document = new Document(pdfFile);
            //    RenderSettings renderSettings = new RenderSettings();
            //    ConvertToWpfOptions renderOptions = new ConvertToWpfOptions { ConvertToImages = false };
            //    renderSettings.RenderPurpose = RenderPurpose.Print;
            //    renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighQuality;
            //    //convert the pdf with the rendersettings and options to a fixed-size document which can be printed
            //    fixedDocument = document.ConvertToWpf(renderSettings, renderOptions);
            //}
            //printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Print");
        }
    }
        
    
}
