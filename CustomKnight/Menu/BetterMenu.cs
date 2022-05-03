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
    internal static class BetterMenu
    {
        internal static int selectedSkin = 0;
        internal static Menu MenuRef;

        internal static void ApplySkin(){
            var skinToApply = SkinManager.SkinsList[selectedSkin];
            SkinManager.SetSkinById(skinToApply.GetId());
            SkinSwapperPanel.hidePanel("");
        }

        internal static void SelectedSkin(string skinId){
            selectedSkin = SkinManager.SkinsList.FindIndex( skin => skin.GetId() == skinId);
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
            //var btn = (MenuRef?.Find("AdditonalButtonGroup") as IShadowElement)?.GetElements()?.FirstOrDefault<Element>( e => e.Id == "DumpButton");
            var btn = (MenuRef?.Find("AdditonalButtonGroup") as MenuRow)?.Find("DumpButton");
            btn.Name = CustomKnight.dumpManager.enabled ? "Dumping sprites" : "Dump sprites";
            btn.Update();
        }
        internal static void ToggleDumping(){
            CustomKnight.dumpManager.enabled = !CustomKnight.dumpManager.enabled;
            SetDumpButton();
        }

        internal static void DumpAll(){
            CustomKnight.dumpManager.enabled = !CustomKnight.dumpManager.enabled;
            CustomKnight.dumpManager.walk();
        }

        private static void OpenSkins(){
            IoUtils.OpenDefault(SkinManager.SKINS_FOLDER);
        }

        private static void OpenLink(string link){ 
            Application.OpenURL(link);
        }
        private static void FixSkins(){ 
            FixSkinStructure.FixSkins();
            TextureCache.clearAllTextureCache(); // clear texture cache
            CustomKnight.Instance.Log("Reapplying Skin");
            // reset skin folder so the same skin can be re-applied
            SkinManager.CurrentSkin = null;
            ApplySkin();
            UpdateSkinList();
        }

        internal static void UpdateSkinList(){
            SkinManager.getSkinNames();
            MenuRef.Find("SelectSkinOption").updateAfter((element)=>{
                ((HorizontalOption)element).Values = getSkinNameArray();
            });
        }
        internal static string[] getSkinNameArray(){
            return SkinManager.SkinsList.Select(s => SkinManager.MaxLength(s.GetName(),CustomKnight.GlobalSettings.NameLength)).ToArray();
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
                    getSkinNameArray(),
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
                        new MenuButton("Dump","Dumps the sprites that Swapper supports (Expect lag)",(_)=>ToggleDumping(),Id:"DumpButton"),
                        //new MenuButton("Generate Cache","Generates Cache for Everything (Can take hours)",(_)=>DumpAll(),Id:"DumpAllButton"),
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
                if(SkinManager.CurrentSkin != null){
                    BetterMenu.SelectedSkin(SkinManager.CurrentSkin.GetId());
                }
            };
            return MenuRef.GetMenuScreen(lastMenu);
        }
    }
}