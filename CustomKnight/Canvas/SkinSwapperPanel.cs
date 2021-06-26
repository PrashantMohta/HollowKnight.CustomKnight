using System;
using System.Collections;
using System.IO;
using HutongGames.PlayMaker.Actions;
using InControl;
using Modding;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CustomKnight.Canvas
{
    public class SkinSwapperPanel
    {
        public static CanvasPanel Panel;

        public static void hidePanel(string bn){
            CustomKnight.GlobalSettings.showMovedText = false;
            if(Panel != null) {
                Panel.SetActive(false, true);
            }
        }
        public static void BuildMenu(GameObject canvas)
        {
            float currentElementPosY = 100f;
            int PanelWidth = 500;
            int PanelHeight = 500;

            int OptionSize = 25;
            int fontSize = (int)(OptionSize * 0.85f);
            int headingSize = 50;
            int headingFontSize = (int)(OptionSize * 0.85f);


            Panel = new CanvasPanel(
                canvas,
                new Vector2(0, currentElementPosY), 
                Vector2.zero,
                new Rect(0, 0, PanelWidth, 60)
            );

            Panel.AddText(
                "Change Skin Text",
                "Custom Knight Has Moved to the\n Mods Menu under Options",
                new Vector2(0, currentElementPosY),
                new Vector2(PanelWidth, headingSize), 
                GUIController.Instance.trajanNormal,
                headingFontSize,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
            currentElementPosY += headingSize;

            GC.Collect();


            Panel.AddButton(
                "help button",
                new Vector2(0, currentElementPosY),
                Vector2.zero,
                hidePanel,
                new Rect(0, currentElementPosY, PanelWidth, OptionSize),
                GUIController.Instance.trajanNormal,
                "Okay",
                fontSize
            );
            currentElementPosY += OptionSize;

            Panel.SetActive(false, true);
            
            Vector2 newPanelSize = new Vector2(PanelWidth, currentElementPosY);
            
            On.HeroController.Pause += OnPause;
            On.HeroController.UnPause += OnUnpause;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
        }

        
        
        private static void OnPause(On.HeroController.orig_Pause orig, HeroController hc)
        {
            if(CustomKnight.GlobalSettings.showMovedText){
                Panel.SetActive(true, false);
            }
            orig(hc);
        }
        
        private static void OnUnpause(On.HeroController.orig_UnPause orig, HeroController hc)
        {
            Panel.SetActive(false, true);
            orig(hc);
        }

        private static void OnSceneChange(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "Menu_Title")
            {
                Panel.SetActive(false, true);
            }
        }

        // Taken from https://stackoverflow.com/questions/56949217/how-to-resize-a-texture2d-using-height-and-width
        private static Texture2D Resize(Texture2D texture2D ,int targetX,int targetY)
        {
            RenderTexture rt=new RenderTexture(targetX, targetY,24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D,rt);
            Texture2D result=new Texture2D(targetX,targetY);
            result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
            result.Apply();
            return result;
        }
        
        private static void Log(object message) => Modding.Logger.Log("[Skin Swapper Panel] " + message);
    }
}