using Core.Infrastructure.Composition;

namespace Core.Infrastructure.Ui.Mvc.Menu
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
