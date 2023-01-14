using System.Diagnostics;
using System.Linq;

namespace WinMan
{
    class Factory
    {
        public static string PassCode { get; set; }

        private static PerformanceCounter _processorCounter;
        private static PerformanceCounter _memoryCounter;

        public static PerformanceCounter ProcessorCounter
        {
            get
            {
                if (_processorCounter == null)
                {
                    var processorCategory = PerformanceCounterCategory.GetCategories()
             .FirstOrDefault(cat => cat.CategoryName == "Processor");

                    var countersInCategory = processorCategory.GetCounters("_Total");
                    _processorCounter = countersInCategory.First(cnt => cnt.CounterName == "% Processor Time");
                }
                return _processorCounter;
            }

        }

        public static PerformanceCounter MemoryCounter
        {
            get
            {
                if (_memoryCounter == null)
                {
                    _memoryCounter = new PerformanceCounter();
                    _memoryCounter.CounterName = "% Committed Bytes In Use";
                    _memoryCounter.CategoryName = "Memory";
                }
                return _memoryCounter;
            }

        }
    }
}
