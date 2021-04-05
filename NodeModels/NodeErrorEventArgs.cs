using System;
using System.Collections.Generic;
using System.Text;

namespace NodeModels
{
    public class NodeErrorEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
