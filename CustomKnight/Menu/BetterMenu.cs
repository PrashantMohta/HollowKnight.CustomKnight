using Modding;
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using InControl;
using Satchel;
using Satchel.BetterMenus;
using CustomKnight.Canvas;

namespace CustomKnight 
{
    public static class BetterMenu
    {
        internal static int selectedSkin = 0;
        internal static Menu MenuRef;

        internal static void ApplySkin(){
            var skinToApply = SkinManager.skinsArr[selectedSkin];
            SkinManager.ChangeSkin(skinToApply);
            // use this when saving so you save to the right settings
            if(GameManager.instance.IsNonGameplayScene()){
                CustomKnight.GlobalSettings.DefaultSkin = skinToApply;
            } else {
                CustomKnight.GlobalSettings.DefaultSkin = skinToApply;
                CustomKnight.SaveSettings.DefaultSkin = skinToApply;
            };
            SkinSwapperPanel.hidePanel("");
        }

        internal static void SelectedSkin(string skinName){
            selectedSkin = SkinManager.skinsArr.FindIndex( skin => skin == skinName);
        }
        internal static void SetPreloadButton(){
            var btn = MenuRef.Find("PreloadButton");
            btn.Name = CustomKnight.GlobalSettings.Preloads ? "Gameplay + Events" : "Gameplay only";
            btn.Update();
        }
        internal static void TogglePreloads(){
            CustomKnight.GlobalSettings.Preloads = !CustomKnight.GlobalSettings.Preloads;
            SetPreloadButton();
        }
        internal static void SetDumpButton(){
            var btn = (MenuRef?.Find("AdditonalButtonGroup") as IShadowElement)?.GetElements()?.FirstOrDefault<Element>( e => e.Id == "DumpButton");
            btn.Name = CustomKnight.dumpManager.enabled ? "Dumping sprites" : "Dump sprites";
            btn.Update();
        }
        internal static void ToggleDumping(){
            CustomKnight.dumpManager.enabled = !CustomKnight.dumpManager.enabled;
            SetDumpButton();
        }

        private static void OpenSkins(){
            IoUtils.OpenDefault(SkinManager.SKINS_FOLDER);
        }

        private static void OpenLink(string link){ 
            Application.OpenURL(link);
        }
        private static void FixSkins(){ 
            FixSkinStructure.FixSkins();
            CustomKnight.Instance.Log("Reapplying Skin");
            // reset skin folder so the same skin can be re-applied
            SkinManager.SKIN_FOLDER = null;
            ApplySkin();
            SkinManager.getSkinNames();
            MenuRef.Find("SelectSkinOption").updateAfter((element)=>{
                ((HorizontalOption)element).Values = SkinManager.skinNamesArr.ToArray();
            });
        }
        internal static Menu PrepareMenu(ModToggleDelegates toggleDelegates){
            return new Menu("Custom Knight",new Element[]{
                Blueprints.CreateToggle(toggleDelegates,"Custom Skins", "", "Enabled","Disabled"),
                new HorizontalOption(
                    "Swapper", "Apply skin to bosses, enemies & areas",
                    new string[] { "Disabled", "Enabled" },
                    (setting) => { CustomKnight.toggleSwap(setting == 1); },
                    () => CustomKnight.swapManager.enabled ? 1 : 0,
                    Id:"SwapperEnabled"),
                new HorizontalOption(
                    "Select Skin", "The skin will be used for current save and any new saves.",
                    SkinManager.skinNamesArr.ToArray(),
                    (setting) => { selectedSkin = setting; },
                    () => selectedSkin,
                    Id:"SelectSkinOption"),
                new MenuButton("PreloadButton","Will Preload objects for modifying events",(_)=>TogglePreloads(),Id:"PreloadButton"),
                new MenuRow(
                    new List<Element>{
                        Blueprints.NavigateToMenu( "Skin List","Opens a list of Skins",()=> SkinsList.GetMenu(MenuRef.menuScreen)),
                        new MenuButton("Apply Skin","Apply The currently selected skin.",(_)=> ApplySkin()),
                    },
                    Id:"ApplyButtonGroup"
                ){ XDelta = 400f},

                new TextPanel("To Add more skins, copy the skins into your Skins folder."),
                new MenuRow(
                    new List<Element>{
                        new MenuButton("Open Folder","Open skins folder",(_)=>OpenSkins()),
                        new MenuButton("Fix / Reload","Fix skin structure and reload",(_)=>FixSkins()),
                        new MenuButton("Need Help?","Join the HK Modding Discord",(_)=>OpenLink("https://discord.gg/J4SV6NFxAA")),
                    },
                    Id:"HelpButtonGroup"
                ){ XDelta = 425f},
                new MenuRow(
                    new List<Element>{
                        new MenuButton("DumpButton","Dumps the sprites that Swapper supports (Expect lag)",(_)=>ToggleDumping(),Id:"DumpButton"),
                        //new MenuButton("Need Help?","Join the HK Modding Discord",(_)=>OpenLink("https://discord.gg/J4SV6NFxAA")),
                    },
                    Id:"AdditonalButtonGroup"
                ){ XDelta = 425f},
                
            });
        }
        internal static MenuScreen GetMenu(MenuScreen lastMenu, ModToggleDelegates? toggleDelegates){
            if(MenuRef == null){
                MenuRef = PrepareMenu((ModToggleDelegates)toggleDelegates);
            }
            MenuRef.OnBuilt += (_,Element) => {
                SetPreloadButton();
                SetDumpButton();
            };
            return MenuRef.GetMenuScreen(lastMenu);
        }
    }
}