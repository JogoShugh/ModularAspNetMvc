namespace Core.Infrastructure.Ui.Mvc.ViewModels
{
    public class CommandAction : ICommandAction
    {
        public string Label { get; set; }
        public string ControllerAction { get; set; }
    }
}