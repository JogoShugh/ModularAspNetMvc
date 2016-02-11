namespace FieldReporting.Infrastructure.Ui.Mvc.ViewModels
{
    public interface ICommandAction
    {
        string Label { get; set; }
        string ControllerAction { get; set; }
    }
}