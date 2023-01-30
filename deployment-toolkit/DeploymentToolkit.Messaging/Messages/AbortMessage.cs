using DeploymentToolkit.Modals;

namespace DeploymentToolkit.Messaging.Messages
{
    public class AbortMessage : IMessage
    {
        public MessageId MessageId => MessageId.AbortDeployment;
        public DeploymentStep DeploymentStep { get; set; }
    }
}
