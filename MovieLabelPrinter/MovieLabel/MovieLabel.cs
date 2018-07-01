using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;

using Movie;
using Printing;

// Movie label

namespace MovieLabel
{
    // MovieLabel class: contains movie label data and high-level drawing routines

    public class MovieLabel : Label
    {
        MovieInfo _Movie;

        // Horizontal margin, per side, percent

        const int MarginHorizontalPercent = 6;

        // Vertical margin, per side, percent

        const int MarginVerticalPercent   = 4;

        public MovieLabel(MovieInfo movie)
        {
            _Movie = movie;
            DocumentName = String.Format("{0} - {1}", _Movie.id, _Movie.title);
        }

        public override void BeginPrint(object sender, PrintEventArgs ev)
        {
        }

        public override void EndPrint(object sender, PrintEventArgs ev)
        {
        }

        // Main printing entry point; called once per label because it's single-paged document

        public override void PrintPage(object sender, PrintPageEventArgs ev)
        {
            // Get printable area and calculate drawing area based on percentage based margins

            // Initially, ev.PageSettings.PrintableArea was used; testing proved that landscape orientation is not taken into account by this property
            // Thus PageBounds is used instead, as it reflects correct page printable area boundaries

            RectangleF printableAreaF = new RectangleF(ev.PageBounds.X, ev.PageBounds.Y, ev.PageBounds.Width, ev.PageBounds.Height);
            RectangleF drawingAreaF =
                new RectangleF
                    (
                        printableAreaF.X + printableAreaF.Width  * MarginHorizontalPercent / 100,
                        printableAreaF.Y + printableAreaF.Height * MarginVerticalPercent   / 100,
                        printableAreaF.Width  * (100 - MarginHorizontalPercent*2) / 100,
                        printableAreaF.Height * (100 - MarginHorizontalPercent*2) / 100
                    );

            Rectangle printableArea = Rectangle.Truncate(printableAreaF);
            Rectangle drawingArea   = Rectangle.Truncate(drawingAreaF);

            // Draw "magic line" at the top-left edge of printable area to fight possible auto-adjustment label printer algorithms

            ev.Graphics.DrawLine(Pens.Black, printableArea.Location.X, printableArea.Location.Y, printableArea.Location.X + 1, printableArea.Location.Y);

            // Create and invoke label layout generator; supply calculated drawing area
            // Layout generator will do the printing within given drawing area, auto-adjusting content size

            MovieLabelLayoutGenerator layoutGenerator = new MovieLabelLayoutGenerator(_Movie);
            layoutGenerator.Draw(ev.Graphics, drawingArea);

            // Report no more pages (we assume single-page labels only)

            ev.HasMorePages = false;
        }
    }
}
