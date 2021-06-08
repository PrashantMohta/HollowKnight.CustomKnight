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

        public static void BuildMenu(GameObject canvas)
        {
            Log("Building Skin Swapper Panel");
            float currentElementPosY = CustomKnight.GlobalSettings.PanelY;
            int PanelWidth = CustomKnight.GlobalSettings.PanelWidth;
            int PanelHeight = CustomKnight.GlobalSettings.PanelHeight;

            int NameLength = CustomKnight.GlobalSettings.NameLength;
            int OptionSize = CustomKnight.GlobalSettings.OptionSize;
            int fontSize = (int)(OptionSize * 0.6f);
            int headingSize = (int)(OptionSize * 1.1f);
            int headingFontSize = (int)(headingSize * 0.85f);

            Texture2D texture2D = new Texture2D(PanelWidth, PanelHeight);
            for (int i = 0; i < PanelWidth; i++)
            {
                for (int j = 0; j < PanelHeight; j++)
                {
                    texture2D.SetPixel(i, j, Color.clear);
                }
            }
            texture2D.Apply();
            Texture2D texture2D2 = new Texture2D(PanelWidth, OptionSize);
            for (int k = 0; k < PanelWidth; k++)
            {
                for (int l = 0; l < OptionSize; l++)
                {
                    texture2D2.SetPixel(k, l, Color.clear);
                }
            }
            texture2D2.Apply();
            Panel = new CanvasPanel(
                canvas,
                texture2D,
                new Vector2(0, currentElementPosY), 
                Vector2.zero,
                new Rect(0, 0, PanelWidth, 60)
            );

            Panel.AddText(
                "Change Skin Text",
                "Change Skin",
                new Vector2(0, currentElementPosY),
                new Vector2(PanelWidth, headingSize), 
                GUIController.Instance.trajanNormal,
                headingFontSize,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
            currentElementPosY += headingSize;

            GC.Collect();

            foreach (string path in Directory.GetDirectories(CustomKnight.DATA_DIR))
            {
                string directoryName = new DirectoryInfo(path).Name;
                string buttonText = directoryName.Length <= NameLength ? directoryName : directoryName.Substring(0,NameLength - 3) + "...";
                
                Panel.AddButton(
                    directoryName,
                    texture2D2,
                    new Vector2(0, currentElementPosY),
                    Vector2.zero,
                    ChangeSkin,
                    new Rect(0, currentElementPosY, PanelWidth, OptionSize),
                    GUIController.Instance.trajanNormal,
                    buttonText,
                    fontSize
                );
                currentElementPosY += OptionSize;
                
                GC.Collect();
            }

            Panel.SetActive(false, true);
            
            Vector2 newPanelSize = new Vector2(PanelWidth, currentElementPosY);
            Panel.ResizeBG(newPanelSize);
            
            On.HeroController.Pause += OnPause;
            On.HeroController.UnPause += OnUnpause;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
        }

        private static void ChangeSkin(string buttonName)
        {
            if(CustomKnight.SKIN_FOLDER == buttonName) { return; } 
                
            CustomKnight.SKIN_FOLDER = buttonName;
            CustomKnight.GlobalSettings.DefaultSkin = buttonName;
            CustomKnight.SaveSettings.DefaultSkin = buttonName;
            GameManager.instance.StartCoroutine(ChangeSkinRoutine());
        }

        private static IEnumerator ChangeSkinRoutine()
        {
            HeroController.instance.GetComponent<SpriteFlash>().flashFocusHeal();
            Panel.SetActive(false, true);
            CustomKnight.Instance.LoadSkin();

            yield return new WaitUntil(() => SpriteLoader.LoadComplete);
            
            Panel.SetActive(true, false);
        }
        
        private static void OnPause(On.HeroController.orig_Pause orig, HeroController hc)
        {
            Panel.SetActive(true, false);
            
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