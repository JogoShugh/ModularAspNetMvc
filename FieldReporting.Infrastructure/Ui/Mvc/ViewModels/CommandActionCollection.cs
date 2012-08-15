using System.Collections.Generic;
using System.ComponentModel;

namespace FieldReporting.Infrastructure.Ui.Mvc.ViewModels
{
    public class CommandActionCollection
    {
        private IEnumerable<ICommandAction> _commandActions = new List<ICommandAction>();

        public IEnumerable<ICommandAction> CommandActions { get { return _commandActions; } set { _commandActions = value; } }

        public void Add(string label, string controllerAction)
        {
            var actions = new List<ICommandAction>(CommandActions)
                              {new CommandAction {Label = label, ControllerAction = controllerAction}};

            _commandActions = actions;
        }
    }
}