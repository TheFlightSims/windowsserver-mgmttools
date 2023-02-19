using System.Collections;

namespace Icinga
{
    public enum ServiceState
    {
        ServiceOK,
        ServiceWarning,
        ServiceCritical,
        ServiceUnknown
    }

    public class CheckResult
    {
        public ServiceState State;
        public string Output;
        public string PerformanceData;
    }

    public interface ICheckPlugin
    {
        CheckResult Check(Hashtable args);
    }
}