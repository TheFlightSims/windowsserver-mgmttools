using System;

namespace MonitoringPluginsForWindows
{
    internal class AppPool
    {
        public string Name { get; private set; }
        public bool AutoStart { get; private set; }
        public bool Enable32bitAppOnWin64 { get; private set; }
        public bool IsLocallyStored { get; private set; }
        public string ManagedPipelineMode { get; private set; }
        public string ManagedRuntimeVersion { get; private set; }
        public long QueueLength { get; private set; }
        public string State { get; private set; }
        public string ProcessModelIdentityType { get; private set; }
        public TimeSpan ProcessModelIdleTimeout { get; private set; }
        public bool ProcessModelLoadUserProfile { get; private set; }
        public long ProcessModelMaxProcesses { get; private set; }
        public bool ProcessModelPingingEnabled { get; private set; }
        public int ProcessModelPingInterval { get; private set; }
        public long ProcessModelPingResponseTime { get; private set; }
        public string ProcessModelUserName { get; private set; }
        public bool CpuSmpAffinitized { get; private set; }
        public string FailureLoadBalancerCapabilities { get; private set; }
        public Array WorkerProcesses { get; private set; }

        public AppPool(string name, bool autoStart, bool enable32bitAppOnWin64, bool isLocallyStored,
            string managedPipelineMode, string managedRuntimeVersion, long queueLength, string state,
            string processModelIdentityType, TimeSpan processModelIdleTimeout,
            bool processModelLoadUserProfile, long processModelMaxProcesses, bool processModelPingingEnabled,
            string processModelUserName, bool cpuSmpAffinitized, string failureLoadBalancerCapabilities,
            Array workerProcesses)
        {
            Name = name;
            AutoStart = autoStart;
            Enable32bitAppOnWin64 = enable32bitAppOnWin64;
            IsLocallyStored = isLocallyStored;
            ManagedPipelineMode = managedPipelineMode;
            ManagedRuntimeVersion = managedRuntimeVersion;
            QueueLength = queueLength;
            State = state;
            ProcessModelIdentityType = processModelIdentityType;
            ProcessModelIdleTimeout = processModelIdleTimeout;
            ProcessModelLoadUserProfile = processModelLoadUserProfile;
            ProcessModelMaxProcesses = processModelMaxProcesses;
            ProcessModelPingingEnabled = processModelPingingEnabled;
            ProcessModelPingResponseTime = ProcessModelPingResponseTime;
            ProcessModelUserName = processModelUserName;
            CpuSmpAffinitized = cpuSmpAffinitized;
            FailureLoadBalancerCapabilities = failureLoadBalancerCapabilities;
            WorkerProcesses = workerProcesses;
        }
    }

    internal class AppPoolWorkerProcesses
    {
        public string AppPoolName { get; private set; }
        public bool IsLocallyStored { get; private set; }
        public string State { get; private set; }
        public Array ApplicationDomains { get; private set; }

        public AppPoolWorkerProcesses(string appPoolName, bool isLocallyStored, string state, Array applicationDomains)
        {
            AppPoolName = appPoolName;
            IsLocallyStored = isLocallyStored;
            State = state;
            ApplicationDomains = applicationDomains;
        }
    }

    internal class AppPoolWPAppDomains
    {
        public string Id { get; private set; }
        public int Idle { get; private set; }
        public bool IsLocallyStored { get; private set; }
        public string PhysicalPath { get; private set; }
        public string VirtualPath { get; private set; }
        public Array AppPoolWorkerProcesses { get; private set; }

        public AppPoolWPAppDomains(string id, int idle, bool isLocallyStored, string physicalPath, string virtualPath,
            Array appPoolWorkerProcesses)
        {
            Id = id;
            Idle = idle;
            IsLocallyStored = isLocallyStored;
            PhysicalPath = physicalPath;
            VirtualPath = virtualPath;
            AppPoolWorkerProcesses = appPoolWorkerProcesses;
        }
    }

}
