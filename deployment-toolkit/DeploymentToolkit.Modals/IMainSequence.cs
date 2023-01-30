using System;

namespace DeploymentToolkit.Modals
{
    public interface IMainSequence
    {
        event EventHandler<SequenceCompletedEventArgs> OnSequenceCompleted;

        ISequence SubSequence { get; }
        void SequenceBegin();
        void SequenceEnd();
    }
}
