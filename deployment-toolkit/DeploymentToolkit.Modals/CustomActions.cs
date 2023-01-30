using DeploymentToolkit.Modals.Actions;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeploymentToolkit.Modals
{
    [XmlRoot(ElementName = "CustomActions")]
    public class CustomActions
    {
        [XmlElement(ElementName = "Action")]
        public List<ActionBase> Actions { get; set; }
    }
}
