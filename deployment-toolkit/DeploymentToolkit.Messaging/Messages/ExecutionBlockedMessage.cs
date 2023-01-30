namespace DeploymentToolkit.Messaging.Messages
{
    public class ExecutionBlockedMessage : IMessage
    {
        public MessageId MessageId => MessageId.ExecutionBlocked;

        public string ExecutableName { get; set; }
    }
}
