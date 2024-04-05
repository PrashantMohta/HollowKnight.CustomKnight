using CustomKnight.NewUI;
using Satchel.BetterMenus;

namespace CustomKnight
{
    internal class AdvancedMenu
    {
        private static Menu MenuRef;
        private static MenuScreen MenuScreenRef;

        internal static void SetDumpButton()
        {
            var btn = MenuRef?.Find("DumpButton");
            btn.Name = CustomKnight.dumpManager.enabled ? "Dumping sprites" : "Dump sprites";
            btn.Update();
        }
        internal static void ToggleDumping()
        {
            CustomKnight.dumpManager.enabled = !CustomKnight.dumpManager.enabled;
            if (CustomKnight.dumpManager.enabled)
            {
                CustomKnight.swapManager.Unload();
                CustomKnight.dumpManager.dumpAllSprites();
            }
            else
            {
                CustomKnight.swapManager.Load();
            }
            SetDumpButton();
        }
        private static void FixSkins()
        {
            FixSkinStructure.FixSkins();
            BetterMenu.ReloadSkins();
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

        private static Menu PrepareMenu()
        {

            var menu = new Menu("CK Advanced Options", new Element[]{
                new HorizontalOption(
                    "Swapper", "Apply skin to bosses, enemies & areas",
                    new string[] { "Disabled", "Enabled" },
                    (setting) => { CustomKnight.toggleSwap(setting == 1); },
                    () => CustomKnight.swapManager.enabled ? 1 : 0,
                    Id:"SwapperEnabled"),
                Blueprints.HorizontalBoolOption(
                    "Preload GameObjects for Events","(restart required)",
                    (v) => {
                        CustomKnight.GlobalSettings.Preloads = v;
                    },
                    () => CustomKnight.GlobalSettings.Preloads),
                Blueprints.HorizontalBoolOption(
                    "Enable Save Huds","(restart required)",
                    (v) => {
                        CustomKnight.GlobalSettings.EnableSaveHuds = v;
                    },
                    () => CustomKnight.GlobalSettings.EnableSaveHuds),
                Blueprints.HorizontalBoolOption(
                    "Enable Pause Menu UI","(restart required)",
                    (v) => {
                        CustomKnight.GlobalSettings.EnablePauseMenu = v;
                        if (!CustomKnight.GlobalSettings.EnablePauseMenu)
                        {
                            UIController.hideMenu();
                        }
                    },
                    () => CustomKnight.GlobalSettings.EnablePauseMenu),
                new MenuRow(
                    new List<Element>{
                        new MenuButton("Fix Skins","Attempts to Fix Skin Structure",(_)=>FixSkins()),
                    },
                    Id:"AdditonalButtonGroup"
                ){ XDelta = 425f},
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
    }
}
