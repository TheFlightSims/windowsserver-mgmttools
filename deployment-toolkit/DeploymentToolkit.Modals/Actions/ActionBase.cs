using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeploymentToolkit.Modals.Actions
{
    public class ActionBase : IOrderedAction
    {
        [XmlIgnore]
        public List<IExecutableAction> Actions { get; set; } = new List<IExecutableAction>();

        [XmlAttribute(AttributeName = "Conditon")]
        public string Condition { get; set; }
        [XmlAttribute(AttributeName = "ExectionOrder")]
        public ExectionOrder ExectionOrder { get; set; }

        [XmlIgnore()]
        public bool ConditionResults { get; set; }
    }
}
