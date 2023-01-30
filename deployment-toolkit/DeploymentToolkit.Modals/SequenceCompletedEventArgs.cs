using System;
using System.Collections.Generic;

namespace DeploymentToolkit.Modals
{
    public class SequenceCompletedEventArgs : EventArgs
    {
        public bool SequenceSuccessful { get; set; }
        public int ReturnCode { get; set; }

        public int CountErrors => SequenceErrors?.Count ?? 0;
        public int CountWarnings => SequenceWarnings?.Count ?? 0;

        public List<Exception> SequenceErrors { get; set; } = new List<Exception>();
        public List<Exception> SequenceWarnings { get; set; } = new List<Exception>();

        public bool ForceRestart { get; set; }
    }
}
