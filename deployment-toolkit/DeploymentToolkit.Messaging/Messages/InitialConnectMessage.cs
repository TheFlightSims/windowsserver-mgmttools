namespace DeploymentToolkit.Messaging.Messages
{
    public class InitialConnectMessage : IMessage
    {
        public MessageId MessageId => MessageId.InitialConnectMessage;

        public int SessionId { get; set; }

        public string Username { get; set; }
        public string Domain { get; set; }
    }
}
