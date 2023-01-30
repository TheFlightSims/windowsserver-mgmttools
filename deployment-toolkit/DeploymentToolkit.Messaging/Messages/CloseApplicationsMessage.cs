namespace DeploymentToolkit.Messaging.Messages
{
    public class CloseApplicationsMessage : IMessage
    {
        public MessageId MessageId => MessageId.CloseApplications;

        public string[] ApplicationNames { get; set; }
        public int TimeUntilForceClose { get; set; }
    }
}
