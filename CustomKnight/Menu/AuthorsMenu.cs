using Satchel.BetterMenus;

namespace CustomKnight
{
    internal class AuthorsMenu
    {
        private static Menu MenuRef;
        private static MenuScreen MenuScreenRef;

        internal static Menu PrepareMenu()
        {
            var menu = new Menu("Author Tools", new Element[]{
            new HorizontalOption(
                    "Dump Style", "Choose dump style, Names.Json is Recommended, Directory is Faster",
                    new string[] { "Names.Json", "Directory" },
                    (setting) => { CustomKnight.GlobalSettings.DumpOldSwaps = setting == 1; },
                    () => CustomKnight.GlobalSettings.DumpOldSwaps ? 1 : 0,
                    Id:"SelectDumpOption"),
               new MenuButton("Dump","Dumps the sprites that Swapper supports (Expect lag)",(_)=>ToggleDumping(),Id:"DumpButton"),
            });

            return menu;
        }

        internal static MenuScreen GetMenu(MenuScreen lastMenu)
        {
            if (MenuScreenRef == null)
            {
                if (MenuRef == null)
                {
                    MenuRef = PrepareMenu();
                }
                MenuRef.OnBuilt += (_, Element) =>
                {
                    SetDumpButton();
                };
                MenuScreenRef = MenuRef.GetMenuScreen(lastMenu);

            }
            else
            {
                MenuRef.returnScreen = lastMenu;
            }
            return MenuScreenRef;
        }


        internal static void SetDumpButton()
        {
            var btn = MenuRef?.Find("DumpButton");
            btn.Name = CustomKnight.dumpManager.GetIsEnabled() ? "Dumping sprites" : "Dump sprites";
            btn.Update();
        }
        internal static void ToggleDumping()
        {
            if (CustomKnight.dumpManager.GetIsEnabled())
            {
                //Disable
                CustomKnight.dumpManager.Unhook();
                CustomKnight.swapManager.Hook();
            }
            else
            {
                //Enable
                CustomKnight.dumpManager.Hook();
                CustomKnight.swapManager.Unhook();
                SkinManager.SetDefaultSkin();
                CustomKnight.dumpManager.DumpAllSprites();
            }
            SetDumpButton();
        }
    }
}
