using CustomKnight.Canvas;
using CustomKnight.NewUI;
using Satchel.BetterMenus;
using System.Linq;

namespace CustomKnight
{
    internal static class BetterMenu
    {
        internal static int selectedSkin = 0;
        internal static Menu MenuRef;

        internal static void ApplySkin()
        {
            var skinToApply = SkinManager.SkinsList[selectedSkin];
            SkinManager.SetSkinById(skinToApply.GetId());
            SkinSwapperPanel.hidePanel("");
        }

        internal static void SelectedSkin(string skinId)
        {
            selectedSkin = SkinManager.SkinsList.FindIndex(skin => skin.GetId() == skinId);
        }

        private static void OpenSkins()
        {
            IoUtils.OpenDefault(SkinManager.SKINS_FOLDER);
        }

        private static void OpenLink(string link)
        {
            Application.OpenURL(link);
        }
        internal static void ReloadSkins()
        {
            CustomKnight.Instance.Log("Reapplying Skin");
            // clear texture cache
            TextureCache.clearAllTextureCache();
            // clear SaveHud cache & reset
            SaveHud.ClearCache();
            // reset skin folder so the same skin can be re-applied
            SkinManager.CurrentSkin = null;
            ApplySkin();
            UpdateSkinList();
        }

        internal static void UpdateSkinList()
        {
            SkinManager.getSkinNames();
            MenuRef.Find("SelectSkinOption").updateAfter((element) =>
            {
                ((HorizontalOption)element).Values = getSkinNameArray();
            });
            UIController.CreateUpdateGUI();
        }

        internal static string[] getSkinNameArray()
        {
            return SkinManager.SkinsList.Select(s => SkinManager.MaxLength(s.GetName(), CustomKnight.GlobalSettings.NameLength)).ToArray();
        }
        internal static Menu PrepareMenu(ModToggleDelegates toggleDelegates)
        {
            return new Menu("Custom Knight", new Element[]{
                Blueprints.CreateToggle(toggleDelegates,"Custom Skins", "", "Enabled","Disabled"),
                new HorizontalOption(
                    "Select Skin", "The skin will be used for current save and any new saves.",
                    getSkinNameArray(),
                    (setting) => { selectedSkin = setting; },
                    () => selectedSkin,
                    Id:"SelectSkinOption"),
                new TextPanel("To Add more skins, copy the skins into your Skins folder."),
                new MenuRow(
                    new List<Element>{
                        Blueprints.NavigateToMenu( "Skin List","Opens a list of Skins",()=> SkinsList.GetMenu(MenuRef.menuScreen)),
                        Blueprints.NavigateToMenu("Alternates","Configure Alternate sheets",() => AltMenu.GetMenu(MenuRef.menuScreen)),
                    },
                    Id:"ApplyButtonGroup"
                ){ XDelta = 400f},
                new MenuButton("Apply Skin","Apply The currently selected skin.",(_)=> ApplySkin()),
                new MenuRow(
                    new List<Element>{
                        new MenuButton("Open Folder","Open skins folder",(_)=>OpenSkins()),
                        new MenuButton("Reload","Reload all skins",(_)=>ReloadSkins()),
                        new MenuButton("Need Help?","Join the HK Modding Discord",(_)=>OpenLink("https://discord.gg/J4SV6NFxAA")),
                    },
                    Id:"HelpButtonGroup"
                ){ XDelta = 425f},
                new MenuRow(
                    new List<Element>{
                        Blueprints.NavigateToMenu("Advanced","Advanced Settings", ()=> AdvancedMenu.GetMenu(MenuRef.menuScreen)),
                        Blueprints.NavigateToMenu("KeyBinds","Hotkey bindings", ()=> KeybindsMenu.GetMenu(MenuRef.menuScreen)),
                        Blueprints.NavigateToMenu("Tools","For skin Authors", ()=> AuthorsMenu.GetMenu(MenuRef.menuScreen)),
                    },
                    Id:"SubMenuGroup"
                ){ XDelta = 425f},
            });
        }
        internal static MenuScreen GetMenu(MenuScreen lastMenu, ModToggleDelegates? toggleDelegates)
        {
            if (MenuRef == null)
            {
                MenuRef = PrepareMenu((ModToggleDelegates)toggleDelegates);
            }
            MenuRef.OnBuilt += (_, Element) =>
            {
                if (SkinManager.CurrentSkin != null)
                {
                    SelectedSkin(SkinManager.CurrentSkin.GetId());
                }
            };
            return MenuRef.GetMenuScreen(lastMenu);
        }
    }
}