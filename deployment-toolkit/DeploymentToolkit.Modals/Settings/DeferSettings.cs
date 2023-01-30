using System;
using System.Xml.Serialization;

namespace DeploymentToolkit.Modals.Settings
{
    public class DeferSettings
    {
        public int Days { get; set; } = -1;
        public string Deadline
        {
            get
            {
                return DeadlineAsDate.ToShortDateString();
            }
            set
            {
                if(!DateTime.TryParse(value, out var date))
                {
                    DeadlineAsDate = DateTime.MinValue;
                }

                DeadlineAsDate = date;
            }
        }
        [XmlIgnore]
        public DateTime DeadlineAsDate { get; private set; }
    }
}
