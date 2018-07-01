using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;

// Label

namespace Printing
{
    // Label class: represents label with its data and drawing procedure

    public abstract class Label
    {
        public string DocumentName { get; set; }

        public abstract void BeginPrint(object sender, PrintEventArgs ev);
        public abstract void EndPrint(object sender, PrintEventArgs ev);
        public abstract void PrintPage(object sender, PrintPageEventArgs ev);
    }
}
