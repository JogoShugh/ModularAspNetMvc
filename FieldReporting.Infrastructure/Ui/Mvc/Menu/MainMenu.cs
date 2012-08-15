using FieldReporting.Infrastructure.Composition;

namespace FieldReporting.Infrastructure.Ui.Mvc.Menu
{
    public class MainMenu
    {
        public PartsList<MenuItemAction> MenuItems { get; set; }

        public MainMenu()
        {
            new PartsAssembler().ComposeParts(MenuItems);
        }


    }
}
