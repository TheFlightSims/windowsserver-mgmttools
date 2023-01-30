using DeploymentToolkit.Modals;

namespace DeploymentToolkit.Messaging.Messages
{
    public class ContinueMessage : IMessage
    {
        public MessageId MessageId => MessageId.ContinueDeployment;
        public DeploymentStep DeploymentStep { get; set; }
    }
}
