using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeploymentToolkit.Modals
{
    [XmlRoot(ElementName = "CustomVariable")]
    public class CustomVariable
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "Script")]
        public string Script { get; set; }
        [XmlAttribute(AttributeName = "Environment")]
        public string Environment { get; set; }
    }

    [XmlRoot(ElementName = "CustomVariables")]
    public class CustomVariables
    {
        [XmlElement(ElementName = "CustomVariable")]
        public List<CustomVariable> Variables { get; set; }
    }
}
