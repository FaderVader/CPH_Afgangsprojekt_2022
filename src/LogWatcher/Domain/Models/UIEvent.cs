using System;

namespace Domain.Models
{
    public class UIEventArg : EventArgs
    {
        public bool BackEndBusy { get; set; }
    }
}
