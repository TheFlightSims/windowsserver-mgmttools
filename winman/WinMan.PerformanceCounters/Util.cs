using System.Diagnostics;
using WinMan.PerformanceCounters.Models;

namespace WinMan.PerformanceCounters
{
    public class Util
    {
        public static PerformanceCounterModel GetPerformanceCounter(PerformanceCounter processorCounter,
            PerformanceCounter memoryCounter)
        {
            var rtnVal = new PerformanceCounterModel();
            rtnVal.Processor = processorCounter.NextValue();
            rtnVal.Memory = memoryCounter.NextValue();
            return rtnVal;
        }
    }
}
