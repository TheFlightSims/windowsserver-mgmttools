namespace DeploymentToolkit.Messaging.Messages
{
    public class DeploymentLogoffMessage : IMessage
    {
        public MessageId MessageId => MessageId.DeploymentLogoff;

        public int TimeUntilForceLogoff { get; set; }
    }
}
