using System;

namespace MonitoringPluginsForWindows
{
    internal class WebSite
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public bool ServerAutoStart { get; private set; }
        public bool IsLocallyStored { get; private set; }
        public string State { get; private set; }
        public string LogFileDirectory { get; private set; }
        public bool LogFileEnabled { get; private set; }
        public Array Bindings { get; private set; }
        public Array Applications { get; private set; }

        public WebSite(long id, string name, bool serverAutoStart, bool isLocallyStored, string state,
            string logFileDirectory, bool logFileEnabled, Array bindings, Array applications)
        {
            Id = id;
            Name = name;
            ServerAutoStart = serverAutoStart;
            IsLocallyStored = isLocallyStored;
            State = state;
            LogFileDirectory = logFileDirectory;
            LogFileEnabled = logFileEnabled;
            Bindings = bindings;
            Applications = applications;
        }
    }

    internal class SiteBinding
    {
        public string Protocol { get; private set; }
        public string BindingInformation { get; private set; }
        public string Host { get; private set; }
        public string CertificateHash { get; private set; }
        public string CertificateStoreName { get; private set; }
        public bool UseDsMapper { get; private set; }
        public bool IsIPPortHostBinding { get; private set; }
        public bool IsLocallyStored { get; private set; }

        public SiteBinding(string protocol, string bindingInformation, string host, string certificateHash,
            string certificateStoreName, bool useDsMapper, bool isIpPortHostBinding, bool isLocallyStored)
        {
            Protocol = protocol;
            BindingInformation = bindingInformation;
            Host = host;
            CertificateHash = certificateHash;
            CertificateStoreName = certificateStoreName;
            UseDsMapper = useDsMapper;
            IsIPPortHostBinding = isIpPortHostBinding;
            IsLocallyStored = isLocallyStored;
        }
    }

    internal class SiteApplications
    {
        public string ApplicationPoolName { get; private set; }
        public string EnabledProtocols { get; private set; }
        public bool IsLocallyStored { get; private set; }
        public string Path { get; private set; }
        public Array VirtualDirectories { get; private set; }

        public SiteApplications(string applicationPoolName, string enabledProtocols, bool isLocallyStored,
            string path, Array virtualDirectories)
        {
            ApplicationPoolName = applicationPoolName;
            EnabledProtocols = enabledProtocols;
            IsLocallyStored = isLocallyStored;
            Path = path;
            VirtualDirectories = virtualDirectories;
        }
    }

    internal class SiteAppVirtualDirectories
    {
        public string Path { get; private set; }
        public string PhysicalPath { get; private set; }
        public bool IsLocallyStored { get; private set; }
        public string LogonMethod { get; private set; }
        public string UserName { get; private set; }

        public SiteAppVirtualDirectories(string path, string physicalPath, bool isLocallyStored,
            string logonMethod, string userName)
        {
            Path = path;
            PhysicalPath = physicalPath;
            IsLocallyStored = isLocallyStored;
            LogonMethod = logonMethod;
            UserName = userName;
        }
    }
}
