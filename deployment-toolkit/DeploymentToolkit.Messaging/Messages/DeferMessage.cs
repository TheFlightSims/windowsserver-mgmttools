using System;

namespace DeploymentToolkit.Messaging.Messages
{
    public class DeferMessage : IMessage
    {
        public MessageId MessageId => MessageId.DeferDeployment;

        public int RemainingDays { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
