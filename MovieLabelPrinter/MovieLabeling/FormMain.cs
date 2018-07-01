using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Utils;
using Movie;
using Printing;
using MovieLabel;

// Main Form

namespace MovieLabelPrinter
{
    public partial class FormMain : Form
    {
        const int _MaxLogEntries = 1000;

        MovieCache _MovieCache = new MovieCache();

        public FormMain()
        {
            InitializeComponent();
        }

        // Do the initialization at form show time: get list of printers

        private void FormMain_Shown(object sender, EventArgs e)
        {
            Log(LogType.Info, LogLevel.Normal, "Application starting");

            Log(LogType.Info, LogLevel.Verbose, "Getting list of installed printers");

            try
            {
                foreach (var printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    cbPrinter.Items.Add(printerName);
            }
            catch (Win32Exception ex)
            {
                Log(LogType.Error, LogLevel.Critical, "Cannot get printer list: {0}", ex.Message);
            }

            if (cbPrinter.Items.Count > 0)
                cbPrinter.SelectedIndex = 0;

            Log(LogType.Info, LogLevel.Verbose, "Initialization complete");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                // Set UI waiting

                Cursor = Cursors.WaitCursor;
                btnPrint.Enabled = false;
                btnClose.Enabled = false;

                Application.DoEvents();

                // Do the job

                PrintLabel();
            }
            catch (Exception ex)
            {
                Log(LogType.Error, LogLevel.Critical, "Internal error: {0}", ex.Message);
            }
            finally
            {
                // Release UI waiting status

                Cursor = Cursors.Arrow;
                btnPrint.Enabled = true;
                btnClose.Enabled = true;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Main printing routine

        private void PrintLabel()
        {
            string movieId;
            string printerName;

            // Get printing parameters from UI

            try
            {
                Log(LogType.Info, LogLevel.Verbose, "Initiating label printing");

                movieId = edMovieId.Text;

                Object selectedPrinter = cbPrinter.SelectedItem;
                if (selectedPrinter == null)
                    throw new MovieLabelingInvalidParameterException("Printer is not selected");

                printerName = selectedPrinter.ToString();

                Log(LogType.Info, LogLevel.Verbose, "Using printer name '{0}' and movie Id '{1}'", printerName, movieId);
            }
            catch (Exception ex)
            {
                Log(LogType.Error, LogLevel.Critical, "{0}", ex.Message);
                return;
            }

            // Get movie data from cache

            MovieInfo movie;

            try
            {
                Log(LogType.Info, LogLevel.Verbose, "Getting movie info for Id '{0}'", movieId);

                Application.DoEvents();

                movie = _MovieCache.Get(movieId);

                Log(LogType.Info, LogLevel.Verbose, "Movie info obtained: {1} - {2}", printerName, movie.id, movie.title);
            }
            catch (Exception ex)
            {
                Log(LogType.Error, LogLevel.Critical, "Cannot download movie: {0}", ex.Message);
                return;
            }

            // Print the label

            try
            {
                Log(LogType.Info, LogLevel.Verbose, "Sending label to printer {0}", printerName);

                var label = new MovieLabel.MovieLabel(movie);

                var lpm = new LabelPrintManager(printerName);

                Application.DoEvents();

                lpm.PrintLabel(label);

                Log(LogType.Info, LogLevel.Normal, "Label sent to printer {0}: {1} - {2}", printerName, movie.id, movie.title);
            }
            catch (Exception ex)
            {
                Log(LogType.Error, LogLevel.Critical, "Cannot print label: {0}", ex.Message);
                return;
            }

        }

        // Logging routine: add message to log list view and scroll it into view if necessary

        private void Log(LogType logType, LogLevel logLevel, string message, params Object[] args)
        {
            lvLog.BeginUpdate();

            // Check if last item is visible

            ListViewItem lastItem = lvLog.Items.Count > 0 ? lvLog.Items[lvLog.Items.Count - 1] : null;

            bool lastItemVisible = (lastItem != null) && (lastItem.Bounds.IntersectsWith(lvLog.ClientRectangle));

            // Add new item to list view

            ListViewItem newItem =
                new ListViewItem(new string[]
                {
                    DateTime.Now.ToString("HH:mm:ss . fff"),
                    logType.ToString(),
                    String.Format(message, args)
                });

            lvLog.Items.Add(newItem);

            // Check if we reached maximum log entries and need cleanup

            if (lvLog.Items.Count > _MaxLogEntries)
            {
                for (int i = 0; i < _MaxLogEntries / 10; i++)
                    lvLog.Items.RemoveAt(0);
            }

            // If user didn't scroll the log up make sure new log entry will be visible for monitoring

            if (lastItemVisible)
                newItem.EnsureVisible();

            lvLog.EndUpdate();
        }
    }
}
