using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using ViewModel.Models;

namespace ViewModel.Helpers
{
    public static class PrintHelper
    {
        //public static FixedDocument GetFixedDocument(FrameworkElement toPrint, PrintDialog printDialog)
        //{
        //    PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
        //    Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
        //    Size visibleSize = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
        //    FixedDocument fixedDoc = new FixedDocument();

        //    // If the toPrint visual is not displayed on screen we neeed to measure and arrange it.
        //    toPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        //    toPrint.Arrange(new Rect(new Point(0, 0), toPrint.DesiredSize));

        //    Size size = toPrint.DesiredSize;

        //    // Will assume for simplicity the control fits horizontally on the page.
        //    double yOffset = 0;
        //    while (yOffset < size.Height)
        //    {
        //        VisualBrush vb = new VisualBrush(toPrint);
        //        vb.Stretch = Stretch.None;
        //        vb.AlignmentX = AlignmentX.Left;
        //        vb.AlignmentY = AlignmentY.Top;
        //        vb.ViewboxUnits = BrushMappingMode.Absolute;
        //        vb.TileMode = TileMode.None;
        //        vb.Viewbox = new Rect(0, yOffset, visibleSize.Width, visibleSize.Height);

        //        PageContent pageContent = new PageContent();
        //        FixedPage page = new FixedPage();
        //        ((IAddChild)pageContent).AddChild(page);
        //        fixedDoc.Pages.Add(pageContent);
        //        page.Width = pageSize.Width;
        //        page.Height = pageSize.Height;

        //        Canvas canvas = new Canvas();
        //        FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
        //        FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
        //        canvas.Width = visibleSize.Width;
        //        canvas.Height = visibleSize.Height;
        //        canvas.Background = vb;
        //        page.Children.Add(canvas);

        //        yOffset += visibleSize.Height;
        //    }
        //    return fixedDoc;
        //}

        //public static void ShowPrintPreview(FixedDocument fixedDoc)
        //{
        //    Window wnd = new Window();
        //    DocumentViewer viewer = new DocumentViewer();
        //    viewer.Document = fixedDoc;
        //    wnd.Content = viewer;
        //    wnd.ShowDialog();
        //}

        public static void PrintViewList(ListView appointmentList)
        {
            PrintDialog pd = new PrintDialog();
            FlowDocument fd = new FlowDocument();
            fd.FontFamily = new FontFamily("Courier New");
            fd.FontSize = 11;
            fd.PagePadding = new Thickness(50);
            fd.ColumnGap = 0;
            fd.ColumnWidth = pd.PrintableAreaWidth;
            fd.Blocks.Add(new Paragraph(new Run(String.Format($"{"Subject",-35}{"Organizer",-15}{"Beginning date",-18}{"Ending date",-18}{"Location",-10}{"User count",-3}"))));
            foreach (var item in appointmentList.ItemContainerGenerator.Items)
            {
                var temp = item as AppointmentModel;
                fd.Blocks.Add(new Paragraph(new Run(String.Format($"{temp.Subject,-35}{temp.AppointmentId,-15}{temp.BeginningDate.ToString("dd-MM-yyyy HH-mm"),-18}{temp.EndingDate.ToString("dd-MM-yyyy HH-mm"),-18}{temp.Room,-15}{temp.Users.Count,-3}"))));
            }
            IDocumentPaginatorSource dps = fd;
            pd.PrintDocument(dps.DocumentPaginator, "flowdoc" );
        }

        public static void PrintAppointment(AppointmentModel appointment)
        {

            PrintDialog pd = new PrintDialog();
            FlowDocument fd = new FlowDocument();
            fd.FontFamily = new FontFamily("Courier New");
            fd.FontSize = 14;
            fd.PagePadding = new Thickness(50);
            fd.ColumnGap = 0;
            fd.ColumnWidth = pd.PrintableAreaWidth;
            fd.Blocks.Add(new Paragraph(new Run(String.Format($"{"Subject:",-15}{appointment.Subject,-50}"))));
            fd.Blocks.Add(new Paragraph(new Run(String.Format($"{"Organizer:",-15}{appointment.AppointmentId,-50}"))));
            fd.Blocks.Add(new Paragraph(new Run(String.Format($"{"Beginning date:",-15}{appointment.BeginningDate.ToString("dd-MM -yyyy HH-mm"),-20}"))));
            fd.Blocks.Add(new Paragraph(new Run(String.Format($"{"Ending date:",-15}{appointment.EndingDate.ToString("dd-MM -yyyy HH-mm"),-20}"))));
            fd.Blocks.Add(new Paragraph(new Run(String.Format($"{"Location:",-15}{appointment.Room,-50}"))));
            fd.Blocks.Add(new Paragraph(new Run("Participant")));
            foreach (var item in appointment.Users)
            {
                fd.Blocks.Add(new Paragraph(new Run(String.Format($"{"Name", -6}{item.Name, -50}"))));
            }
            IDocumentPaginatorSource dps = fd;
            pd.PrintDocument(dps.DocumentPaginator, "flowdoc");
        }
    }
}
