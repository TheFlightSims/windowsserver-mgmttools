using System;

namespace DeploymentToolkit.Modals
{
    public class DeferedDeployment
    {
        public string Name { get; set; }
        public int RemainingDays { get; set; } = -1;
        public DateTime Deadline { get; set; } = DateTime.MinValue;
    }
}
