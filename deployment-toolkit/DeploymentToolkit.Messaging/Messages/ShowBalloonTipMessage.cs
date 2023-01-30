using System.Windows.Forms;

namespace DeploymentToolkit.Messaging.Messages
{
    public class ShowBalloonTipMessage : IMessage
    {
        public MessageId MessageId => MessageId.ShowBalloonTip;

        public string Title { get; set; }
        public string Message { get; set; }
        public int TimeOut { get; set; }
        public ToolTipIcon Icon { get; set; }
    }
}
