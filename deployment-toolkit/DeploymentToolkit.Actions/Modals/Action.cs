using DeploymentToolkit.Modals.Actions;
using NLog;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace DeploymentToolkit.Actions.Modals
{
    [XmlRoot(ElementName = "Action")]
    public class Action : ActionBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Action(ActionBase actionBase)
        {
            Actions = actionBase.Actions.ToList();

            Condition = actionBase.Condition;
            ExectionOrder = actionBase.ExectionOrder;

            ConditionResults = actionBase.ConditionResults;
        }

        public void ExecuteActions()
        {
            _logger.Trace($"Starting execution of {Actions.Count} actions ...");

            foreach(var action in Actions)
            {
                _logger.Trace($"Executing {action.GetType().Name} ...");
                try
                {
                    action.Execute();
                }
                catch(Exception ex)
                {
                    _logger.Warn(ex, "Exeuction of action failed");
                }
            }

            _logger.Trace($"Execution successfully finished");
        }
    }
}
