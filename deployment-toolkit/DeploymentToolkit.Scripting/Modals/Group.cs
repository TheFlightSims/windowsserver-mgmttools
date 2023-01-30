using DeploymentToolkit.Scripting.Exceptions;
using System;
using System.Collections.Generic;

namespace DeploymentToolkit.Scripting.Modals
{
    public class Group
    {
        public Guid ID { get; } = Guid.NewGuid();

        public Condition Condition { get; set; }
        public List<Group> SubGroups { get; set; } = new List<Group>();

        public List<GroupLink> GroupLinks { get; set; } = new List<GroupLink>();

        public bool IsTrue()
        {
            if(GroupLinks.Count > 0)
            {
                var isTrue = false;

                var currentGroup = GroupLinks[0];
                if(GroupLinks.Count == 1)
                {
                    return currentGroup.IsTrue();
                }
                else
                {
                    isTrue = currentGroup.IsTrue();
                    var nextGroupCount = 1;
                    do
                    {
                        var nextGroup = GroupLinks[nextGroupCount++];
                        if(nextGroup.LinkType == LinkType.Or && isTrue)
                        {
                            // The next connection is an OR connection but we already validated a true result
                            return true;
                        }
                        else if(nextGroup.LinkType == LinkType.And)
                        {
                            // The next griup is an AND connection so we need to further validate
                            isTrue = nextGroup.IsTrue();
                        }
                        else
                        {
                            // Its an OR connection but our current link didn't return true so validate further
                        }
                    }
                    while(nextGroupCount < GroupLinks.Count);

                    return isTrue;
                }
            }
            else if(SubGroups.Count > 0)
            {
                if(SubGroups.Count > 1)
                {
                    throw new ScriptingInvalidGroupException("Got more than 1 subgroup but no conditions linkted to them");
                }

                return SubGroups[0].IsTrue();
            }
            else
            {
                return Condition.IsTrue();
            }
        }
    }
}
