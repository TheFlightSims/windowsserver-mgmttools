using System;

namespace WinMan.Network.Models
{
    public class NICModel
    {
        public string Name { get; set; }
        public UInt64 BytesReceivedPersec { get; set; }
        public UInt64 BytesSentPersec { get; set; }
        public UInt64 BytesTotalPersec { get; set; }
        public string Caption { get; set; }
        public UInt64 CurrentBandwidth { get; set; }
        public string Description { get; set; }
        public UInt64 PacketsPersec { get; set; }
        public UInt64 PacketsReceivedPersec { get; set; }
        public UInt64 PacketsSentPersec { get; set; }
    }
}
