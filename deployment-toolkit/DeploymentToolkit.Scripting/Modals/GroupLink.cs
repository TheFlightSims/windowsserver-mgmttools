using DeploymentToolkit.Scripting.Exceptions;

namespace DeploymentToolkit.Scripting.Modals
{
    public enum LinkType
    {
        None,
        And,
        Or
    }
    public class GroupLink
    {
        public LinkType LinkType { get; set; }
        public Group FirstGroup { get; set; }
        public Group SecondGroup { get; set; }

        public GroupLink(Group firstGroup, LinkType linkType, Group secondGroup)
        {
            if(linkType == LinkType.None)
            {
                throw new ScriptingInvalidOperatorException($"{linkType} is not a valid operator");
            }

            this.FirstGroup = firstGroup;
            this.LinkType = linkType;
            this.SecondGroup = secondGroup;
        }

        public bool IsTrue()
        {
            if(LinkType == LinkType.And)
            {
                return FirstGroup.IsTrue() && SecondGroup.IsTrue();
            }
            else if(LinkType == LinkType.Or)
            {
                return FirstGroup.IsTrue() || SecondGroup.IsTrue();
            }

            // Can never happen
            return false;
        }

        public bool IsLinkedToGroup(Group group)
        {
            return FirstGroup == group || SecondGroup == group;
        }
    }
}
