using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using Satchel;
using Satchel.BetterMenus;
using MenuOptionHorizontal = UnityEngine.UI.MenuOptionHorizontal;

namespace CustomKnight 
{
    public static class SkinsList
    {
        internal static Menu MenuRef;
        internal static MenuScreen lastMenu;

        private static bool applying = false;

        private static IEnumerator goBack()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.instance.UIGoToDynamicMenu(lastMenu);
        }

        internal static MenuButton ApplySkinButton(int index){
            return new MenuButton(SkinManager.skinNamesArr[index],"",(mb) => {
                    if(!applying){
                        applying = true;
                        // apply the skin
                        BetterMenu.selectedSkin = index;
                        BetterMenu.ApplySkin();
                        BetterMenu.MenuRef?.Find("SelectSkinOption")?.gameObject?.GetComponent<MenuOptionHorizontal>()?.menuSetting?.RefreshValueFromGameSettings();
                        GameManager.instance.StartCoroutine(goBack());
                    }
                });
        }
        internal static Menu PrepareMenu(){
            var menu = new Menu("Select a skin",new Element[]{});
            menu.AddElement(
                new TextPanel("Select the Skin to Apply")
            );
            for(var i = 0; i < SkinManager.skinNamesArr.Count ; i++){
                menu.AddElement(ApplySkinButton(i));
            }
            return menu;
        }
        internal static MenuScreen GetMenu(MenuScreen lastMenu){
            if(MenuRef == null){
                MenuRef = PrepareMenu();
            }
            applying = false;
            SkinsList.lastMenu = lastMenu;
            return MenuRef.GetMenuScreen(lastMenu);
        }
    }
}