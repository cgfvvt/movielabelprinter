using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;

// Print manager

namespace Printing
{
    // LabelPrintManager class: peforms printing of label to specified printer

    public class LabelPrintManager
    {
        string _PrinterName;
        public string PrinterName
        {
            get
            {
                return _PrinterName;
            }
        }

        public LabelPrintManager(string printerName)
        {
            _PrinterName = printerName;
        }

        public void PrintLabel(Label label)
        {
            // Use PrintDocument class for printing

            PrintDocument document = new PrintDocument();

            // Set up printing delegates

            document.BeginPrint += label.BeginPrint;
            document.EndPrint += label.EndPrint;
            document.PrintPage += label.PrintPage;

            // Define document name

            document.DocumentName = label.DocumentName;

            // Select printer

            document.PrinterSettings = new PrinterSettings();
            document.PrinterSettings.PrinterName = _PrinterName;

            // Do printing (delegates will be called to actually draw the label)

            document.Print();
        }

        public PageSettings GetDefaultPageSettings()
        {
            var settings = new PrinterSettings();

            settings.PrinterName = _PrinterName;
            PageSettings pageSettings = settings.DefaultPageSettings;

            return pageSettings;
        }
    }
}
