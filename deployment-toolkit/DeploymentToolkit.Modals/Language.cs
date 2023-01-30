using System.Xml.Serialization;

namespace DeploymentToolkit.Modals
{
    [XmlRoot("Messages")]
    public class Language
    {
        // Everything below here we add ourselfs
        public string PathToUpgradeRtf { get; set; }

        public string DTK_WindowTitle { get; set; }
        public string DTK_BrandingTitle { get; set; }

        // Upgrade Messages
        public string UpgradePrompt_ButtonStartNow { get; set; }
        public string UpgradePrompt_ButtonSchedule { get; set; }
        public string UpgradePrompt_ButtonMinimize { get; set; }

        // Upgrade Schedule Dialog
        public string UpgradePrompt_ButtonConfirm { get; set; }
        public string UpgradePrompt_ButtonCancel { get; set; }
        public string UpgradePrompt_ScheduleTitle { get; set; }
        public string UpgradePrompt_ScheduleOptionOne { get; set; }
        public string UpgradePrompt_ScheduleOptionTwo { get; set; }
        public string UpgradePrompt_ScheduleReminder { get; set; }


        // Everything below this comment is from PSADT
        public string DiskSpace_Message { get; set; }

        public string ClosePrompt_ButtonContinue { get; set; }
        public string ClosePrompt_ButtonContinueTooltip { get; set; }
        public string ClosePrompt_ButtonClose { get; set; }
        public string ClosePrompt_ButtonDefer { get; set; }
        public string ClosePrompt_Message { get; set; }
        public string ClosePrompt_CountdownMessage { get; set; }

        public string DeferPrompt_WelcomeMessage { get; set; }
        public string DeferPrompt_ExpiryMessage { get; set; }
        public string DeferPrompt_WarningMessage { get; set; }
        public string DeferPrompt_RemainingDeferrals { get; set; }
        public string DeferPrompt_Deadline { get; set; }

        public string WelcomePrompt_CountdownMessage { get; set; }
        public string WelcomePrompt_CustomMessage { get; set; }

        public string DeploymentType_Install { get; set; }
        public string DeploymentType_UnInstall { get; set; }

        public string BalloonText_Start { get; set; }
        public string BalloonText_Complete { get; set; }
        public string BalloonText_RestartRequired { get; set; }
        public string BalloonText_Error { get; set; }
        public string BalloonText_FastRetry { get; set; }
        public string Progress_MessageInstall { get; set; }
        public string Progress_MessageUninstall { get; set; }

        public string BlockExecution_Message { get; set; }

        public string RestartPrompt_Title { get; set; }
        public string RestartPrompt_Message { get; set; }
        public string RestartPrompt_MessageTime { get; set; }
        public string RestartPrompt_MessageRestart { get; set; }
        public string RestartPrompt_TimeRemaining { get; set; }
        public string RestartPrompt_ButtonRestartLater { get; set; }
        public string RestartPrompt_ButtonRestartNow { get; set; }
    }
}
