using System;

namespace WinMan.Process.Models
{
    public class ProcessModel
    {
        public int Id { get; set; }
        public string MainWindowTitle { get; set; }
        public string ProcessName { get; set; }
        public int SessionId { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan TotlaProcessorTime { get; set; }
        public TimeSpan UserProcessorTime { get; set; }
        public long WorkingSet64 { get; set; }
        public long PeakWorkingSet64 { get; set; }
        public string WorkingSet64View { get; set; }
        public string PeakWorkingSet64View { get; set; }
    }
}
