using Satchel.BetterMenus;

namespace CustomKnight
{
    internal class KeybindsMenu
    {
        private static Menu MenuRef;
        private static MenuScreen MenuScreenRef;

        internal static Menu PrepareMenu()
        {
            var menu = new Menu("Keybinds", new Element[]{
                new KeyBind("Pause Menu HotKey" ,CustomKnight.GlobalSettings.Keybinds.OpenSkinList,Id:"PauseMenuHotkey"),
                new KeyBind("Reload Skins HotKey" ,CustomKnight.GlobalSettings.Keybinds.ReloadSkins,Id:"ReloadSkinsHotkey"),
            });
            return menu;
        }

        internal static MenuScreen GetMenu(MenuScreen lastMenu)
        {
            if (MenuScreenRef == null)
            {
                MenuRef ??= PrepareMenu();
                MenuScreenRef = MenuRef.GetMenuScreen(lastMenu);

            }
            else
            {
                MenuRef.returnScreen = lastMenu;
            }
            return MenuScreenRef;
        }
    }
}
