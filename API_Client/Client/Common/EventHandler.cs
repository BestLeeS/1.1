using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public delegate void PrintEventHandler(object sender, PrintEventArgs e);
    public delegate void SaveEventHandler(object sender, SaveEventArgs e);
}
