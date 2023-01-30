namespace DeploymentToolkit.Messaging.Messages
{
    public class DeploymentRestartMessage : IMessage
    {
        public MessageId MessageId => MessageId.DeploymentRestart;

        public int TimeUntilForceRestart { get; set; }
    }
}
