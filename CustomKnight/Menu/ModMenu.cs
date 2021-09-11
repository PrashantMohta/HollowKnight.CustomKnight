using System;
using System.Collections;
using System.Collections.Generic;

using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using Patch = Modding.Patches;
using UnityEngine;
using UnityEngine.UI;
using CustomKnight.Canvas;

namespace CustomKnight{
    public class ModMenu{
        private static MenuScreen modsMenu;
        public static Image previewImage;

        private static ModToggleDelegates stateToggle;

        public static string[] modes = {"Gameplay", "Gameplay + Events"};


        static int selectedSkinIndex = 0;
        static int selectedMode = 1;
        public static int getSkinValue(){
            return selectedSkinIndex;
        }

        public static void setSkinValue(int val){
            selectedSkinIndex = val;
        }


        public static int getModeValue(){
            return selectedMode;
        }

        public static void setModeValue(int val){
            selectedMode = val;
        }

        public static MenuOptionHorizontal skinSelector;
        public static MenuOptionHorizontal modeSelector;
        public static MenuOptionHorizontal stateSelector;
        public static MenuOptionHorizontal SwapperEnabled,SwapperDumpEnabled;
        private static void addMenuOptions(ContentArea area){
      
            
            area.AddHorizontalOption(
                    "State",
                    new HorizontalOptionConfig
                    {
                        Options = new string[] { "Disabled", "Enabled" },
                        ApplySetting = (_, i) => stateToggle.SetModEnabled(i == 1),
                        RefreshSetting = (s, _) => s.optionList.SetOptionTo(stateToggle.GetModEnabled() ? 1 : 0),
                               
                        CancelAction = GoToModListMenu,
                        Description = new DescriptionInfo
                        {
                            Text = "Allows disabling custom skins",
                            Style = DescriptionStyle.HorizOptionSingleLineVanillaStyle
                        },
                        Label = "Custom Skins",
                        Style = HorizontalOptionStyle.VanillaStyle
                    },
                    out stateSelector
                ); 
            area.AddTextPanel("HelpText",
                    new RelVector2(new Vector2(800f, 105f)),
                    new TextPanelConfig{
                        Text = "To Add more skins, copy the skins into your Mods/CustomKnight/Skins/ folder",
                        Size = 25,
                        Font = TextPanelConfig.TextFont.TrajanRegular,
                        Anchor = TextAnchor.MiddleCenter
                    });

            area.AddHorizontalOption(
                    "Select Skin",
                    new HorizontalOptionConfig
                    {
                        ApplySetting = (_, i) => setSkinValue(i),
                        RefreshSetting = (s, _) => s.optionList.SetOptionTo(getSkinValue()),
                        CancelAction = GoToModListMenu,
                        Description = new DescriptionInfo
                        {
                            Text = "The skin will be used for current save and any new saves.",
                            Style = DescriptionStyle.HorizOptionSingleLineVanillaStyle
                        },
                        Label = "Select Skin",
                        Options = SkinManager.skinNamesArr,
                        Style = HorizontalOptionStyle.VanillaStyle
                    },
                    out skinSelector
                ); 
            
            area.AddHorizontalOption(
                    "Mode",
                    new HorizontalOptionConfig
                    {
                        ApplySetting = (_, i) => setModeValue(i),
                        RefreshSetting = (s, _) => s.optionList.SetOptionTo(getModeValue()),
                        CancelAction = GoToModListMenu,
                        Description = new DescriptionInfo
                        {
                            Text = "Will Preload objects for modifying events",
                            Style = DescriptionStyle.HorizOptionSingleLineVanillaStyle
                        },
                        Label = "Mode",
                        Options = modes,
                        Style = HorizontalOptionStyle.VanillaStyle
                    },
                    out modeSelector
                ); 

             area.AddTextPanel("HelpText2",
                    new RelVector2(new Vector2(850f, 105f)),
                    new TextPanelConfig{
                        Text = "In case your skins wont work, try the buttons below",
                        Size = 25,
                        Font = TextPanelConfig.TextFont.TrajanRegular,
                        Anchor = TextAnchor.MiddleCenter
                    });

            area.AddMenuButton(
                        "FixSkinButton",
                        new MenuButtonConfig
                        {
                            Label = "Fix my skins!",
                            CancelAction = GoToModListMenu,
                            SubmitAction = FixSkins,
                            Proceed = true,
                            Style = MenuButtonStyle.VanillaStyle
                        },
                        out MenuButton FixSkinButton
                    );  
            
            area.AddMenuButton(
                        "DiscordButton",
                        new MenuButtonConfig
                        {
                            Label = "Discord Help me!",
                            CancelAction = GoToModListMenu,
                            SubmitAction = OpenLink,
                            Proceed = true,
                            Style = MenuButtonStyle.VanillaStyle
                        },
                        out MenuButton DiscordButton
                    );    


             area.AddTextPanel("HelpText3",
                    new RelVector2(new Vector2(850f, 105f)),
                    new TextPanelConfig{
                        Text = "Experimental features",
                        Size = 35,
                        Font = TextPanelConfig.TextFont.TrajanRegular,
                        Anchor = TextAnchor.MiddleCenter
                    });  
                    
             area.AddTextPanel("HelpText4",
                    new RelVector2(new Vector2(850f, 105f)),
                    new TextPanelConfig{
                        Text = "Note: after enabling Swapper, a restart is Recommended & Dumping resets on restart",
                        Size = 25,
                        Font = TextPanelConfig.TextFont.TrajanRegular,
                        Anchor = TextAnchor.MiddleCenter
                    });  
            area.AddStaticPanel("spacer2", new RelVector2(new Vector2(800f, 105f)),out GameObject spacer2);

            area.AddHorizontalOption(
                    "Swapper",
                    new HorizontalOptionConfig
                    {
                        Options = new string[] { "Disabled", "Enabled" },
                        ApplySetting = (_, i) => {CustomKnight.toggleSwap(i == 1);},
                        RefreshSetting = (s, _) => s.optionList.SetOptionTo(CustomKnight.swapManager.enabled ? 1 : 0),
                               
                        CancelAction = GoToModListMenu,
                        Description = new DescriptionInfo
                        {
                            Text = "allows skinning any tk2dsprite, for example bosses & enemies",
                            Style = DescriptionStyle.HorizOptionSingleLineVanillaStyle
                        },
                        Label = "Swapper",
                        Style = HorizontalOptionStyle.VanillaStyle
                    },
                    out SwapperEnabled
                );

            area.AddHorizontalOption(
                    "DumpTex",
                    new HorizontalOptionConfig
                    {
                        Options = new string[] { "Disabled", "Enabled" },
                        ApplySetting = (_, i) => {CustomKnight.toggleDump(i == 1);},
                        RefreshSetting = (s, _) => s.optionList.SetOptionTo(CustomKnight.dumpManager.enabled ? 1 : 0),
                               
                        CancelAction = GoToModListMenu,
                        Description = new DescriptionInfo
                        {
                            Text = "Dumps the sprites that Swapper supports (Massive lag)",
                            Style = DescriptionStyle.HorizOptionSingleLineVanillaStyle
                        },
                        Label = "Dump Textures",
                        Style = HorizontalOptionStyle.VanillaStyle
                    },
                    out SwapperDumpEnabled
                );

           
        }
        
        public static void GoToModListMenu(object _) {
            GoToModListMenu();
            RefreshOptions();
        }
        public static void GoToModListMenu() => (UIManager.instance).UIGoToDynamicMenu(modsMenu);

        private static void OpenLink(object _) => OpenLink();

        private static void OpenLink(){ 
            Application.OpenURL("https://discord.com/invite/rqsRHRt25h");
        }
        private static void FixSkins(object _) => FixSkins();

        private static void FixSkins(){ 
            SkinManager.fixSkinStructures();
        }
        private static void apply(object _) => apply();

        private static void apply(){ 
            var skinToApply = SkinManager.skinsArr[selectedSkinIndex];
            CustomKnight.Instance.Log("Aplying Settings");
            // apply the skin
            SkinManager.ChangeSkin(skinToApply);
            CustomKnight.GlobalSettings.Preloads = selectedMode == 1;

            // use this when saving so you save to the right settings
            if(GameManager.instance.IsNonGameplayScene()){
                CustomKnight.GlobalSettings.DefaultSkin = skinToApply;
            } else {
                CustomKnight.GlobalSettings.DefaultSkin = skinToApply;
                CustomKnight.SaveSettings.DefaultSkin = skinToApply;
            };

            SkinSwapperPanel.hidePanel("");
            stateToggle.ApplyChange();
            GoToModListMenu();
        }

        public static void RefreshOptions(){
            
            if(skinSelector != null){
                skinSelector.menuSetting.RefreshValueFromGameSettings();
            }
            if(modeSelector != null){
                modeSelector.menuSetting.RefreshValueFromGameSettings();
            }
            if(stateSelector != null){
                stateSelector.menuSetting.RefreshValueFromGameSettings();
            }

            if(SwapperEnabled != null){
                SwapperEnabled.menuSetting.RefreshValueFromGameSettings();
            }
            if(SwapperDumpEnabled != null){
                SwapperDumpEnabled.menuSetting.RefreshValueFromGameSettings();
            }
        }
        public static void setModMenu(string skin,bool preloads){
            if(SkinManager.skinsArr != null){
                var i=0;
                foreach (string curskin in SkinManager.skinsArr)
                {
                    if(skin == curskin){
                        selectedSkinIndex = i;
                        break;
                    }
                    i+=1;
                }
            }
            selectedMode = preloads ? 1 : 0;
            RefreshOptions();
        }

        public static MenuScreen createMenuScreen(MenuScreen modListMenu,ModToggleDelegates? toggle){
            modsMenu = modListMenu;
            stateToggle = toggle.Value;

            string name = "Custom Knight";
            MenuButton applyButton = null;
            MenuButton backButton = null;

            var builder = new MenuBuilder(UIManager.instance.UICanvas.gameObject, name)
                .CreateTitle("Custom Knight", MenuTitleStyle.vanillaStyle)
                .CreateContentPane(RectTransformData.FromSizeAndPos(
                    new RelVector2(new Vector2(1920f, 803f)),
                    new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, -60f)
                    )
                ))
                .CreateControlPane(RectTransformData.FromSizeAndPos(
                    new RelVector2(new Vector2(1920f, 259f)),
                    new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, -502f)
                    )
                ))
                .SetDefaultNavGraph(new ChainedNavGraph())
                .AddControls(
                    new SingleContentLayout(new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, 0f)
                    )),
                    c => c.AddMenuButton(
                        "ApplyButton",
                        new MenuButtonConfig
                        {
                            Label = "Apply",
                            CancelAction = GoToModListMenu,
                            SubmitAction = apply,
                            Proceed = true,
                            Style = MenuButtonStyle.VanillaStyle
                        },
                        out applyButton
                    )
                )
                .AddControls(
                    new SingleContentLayout(new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, -64f)
                    )),
                    c => c.AddMenuButton(
                        "BackButton",
                        new MenuButtonConfig
                        {
                            Label = "Back",
                            CancelAction = GoToModListMenu,
                            SubmitAction = GoToModListMenu,
                            Proceed = true,
                            Style = MenuButtonStyle.VanillaStyle
                        },
                        out backButton
                    )
                );
            builder.AddContent(new NullContentLayout(), c => c.AddScrollPaneContent(
                new ScrollbarConfig
                {
                    CancelAction = _ => (UIManager.instance).UIGoToDynamicMenu(modsMenu),
                    Navigation = new Navigation
                    {
                        mode = Navigation.Mode.Explicit,
                        //selectOnUp = backButton,
                        //selectOnDown = applyButton
                    },
                    Position = new AnchoredPosition
                    {
                        ChildAnchor = new Vector2(0f, 1f),
                        ParentAnchor = new Vector2(1f, 1f),
                        Offset = new Vector2(-310f, 0f)
                    }
                },
                new RelLength(1600f), 
                RegularGridLayout.CreateVerticalLayout(105f),
                contentArea => addMenuOptions(contentArea)
            ));
            return builder.Build();
        }

    }
}