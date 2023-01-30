namespace DeploymentToolkit.Messaging.Messages
{
    public class BasicMessage : IMessage
    {
        public MessageId MessageId { get; set; }

        public BasicMessage()
        {

        }

        public BasicMessage(MessageId messageId)
        {
            MessageId = messageId;
        }
    }
}
